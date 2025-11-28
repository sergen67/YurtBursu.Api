using Microsoft.EntityFrameworkCore;
using YurtBursu.Api.Data;
using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public class BursRepository : IBursRepository
	{
		private readonly AppDbContext _dbContext;

		public BursRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<BursHistory>> GetHistoryByStudentAsync(int studentId, CancellationToken cancellationToken = default)
		{
			return await _dbContext.BursHistories
				.AsNoTracking()
				.Include(b => b.Student)
				.Where(b => b.StudentId == studentId)
				.OrderByDescending(b => b.Year).ThenByDescending(b => b.Month)
				.ToListAsync(cancellationToken);
		}

		public async Task SaveOrOverwriteAsync(BursHistory history, CancellationToken cancellationToken = default)
		{
			var existing = await _dbContext.BursHistories
				.FirstOrDefaultAsync(b => b.StudentId == history.StudentId && b.Year == history.Year && b.Month == history.Month, cancellationToken);

			if (existing is not null)
			{
				_dbContext.BursHistories.Remove(existing);
				await _dbContext.SaveChangesAsync(cancellationToken);
			}

			await _dbContext.BursHistories.AddAsync(history, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}


