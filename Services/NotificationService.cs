using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using YurtBursu.Api.Repositories;

namespace YurtBursu.Api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IConfiguration _configuration;
        private static bool _initialized = false;
        private static readonly object _lock = new();

        public NotificationService(INotificationRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        /// <summary>
        /// FirebaseAdmin 3.4.0 + .NET 8 uyumlu initialization
        /// (FirebaseApp.GetApps() kullanılmaz)
        /// </summary>
        private static void InitializeFirebase(IConfiguration configuration)
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                // Try JSON inline
                var json = configuration["FIREBASE_CREDENTIALS_JSON"];
                GoogleCredential credential = null;

                if (!string.IsNullOrWhiteSpace(json))
                {
                    credential = GoogleCredential.FromJson(json);
                }
                else
                {
                    // Try path
                    var path = configuration["FIREBASE_CREDENTIALS_FILE"];
                    if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                    {
                        credential = GoogleCredential.FromFile(path);
                    }
                }

                if (credential == null)
                {
                    // throw new InvalidOperationException("Firebase credentials missing. Set FIREBASE_CREDENTIALS_JSON or FIREBASE_CREDENTIALS_FILE.");
                    // Don't throw to allow app startup without firebase credentials
                    return; 
                }

                try 
                {
                    // FirebaseAdmin 3.4.0 — DEFAULT INSTANCE sorunsuz
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = credential
                    });
                    _initialized = true;
                }
                catch
                {
                    // Ignore if already exists or invalid
                }
            }
        }

        public async Task SaveTokenAsync(int studentId, string token, CancellationToken cancellationToken = default)
        {
            InitializeFirebase(_configuration);
            if (_initialized) 
                await _repo.SaveTokenAsync(studentId, token, cancellationToken);
        }

        public async Task<int> SendNotificationToAllAsync(string title, string body, CancellationToken cancellationToken = default)
        {
            InitializeFirebase(_configuration);
            if (!_initialized) return 0;

            var tokens = await _repo.GetAllTokensAsync(cancellationToken);
            var count = 0;

            foreach (var token in tokens)
            {
                try 
                {
                    await SendNotificationToTokenAsync(token, title, body, cancellationToken);
                    count++;
                }
                catch
                {
                    // Ignore individual send errors
                }
            }

            return count;
        }

        public async Task SendNotificationToTokenAsync(string token, string title, string body, CancellationToken cancellationToken = default)
        {
            // InitializeFirebase(_configuration); // Already initialized in caller
            var message = new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
        }
    }
}
