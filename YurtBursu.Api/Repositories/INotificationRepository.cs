using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public interface INotificationRepository
	{
		Task<NotificationToken> SaveTokenAsync(int studentId, string token, CancellationToken cancellationToken = default);
		Task<List<string>> GetAllTokensAsync(CancellationToken cancellationToken = default);
	}
}


