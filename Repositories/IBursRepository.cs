using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public interface IBursRepository
	{
		Task<List<BursHistory>> GetHistoryByStudentAsync(int studentId, CancellationToken cancellationToken = default);
		Task SaveOrOverwriteAsync(BursHistory history, CancellationToken cancellationToken = default);
	}
}


