using Microsoft.EntityFrameworkCore;
using YurtBursu.Api.Data;
using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly AppDbContext _db;
		public NotificationRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task<NotificationToken> SaveTokenAsync(int studentId, string token, CancellationToken cancellationToken = default)
		{
			// upsert by token
			var existing = await _db.NotificationTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
			if (existing is not null)
			{
				existing.StudentId = studentId;
				await _db.SaveChangesAsync(cancellationToken);
				return existing;
			}

			var entity = new NotificationToken { StudentId = studentId, Token = token, CreatedAt = DateTime.UtcNow };
			await _db.NotificationTokens.AddAsync(entity, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
			return entity;
		}

		public async Task<List<string>> GetAllTokensAsync(CancellationToken cancellationToken = default)
		{
			return await _db.NotificationTokens.AsNoTracking().Select(x => x.Token).ToListAsync(cancellationToken);
		}
	}
}
