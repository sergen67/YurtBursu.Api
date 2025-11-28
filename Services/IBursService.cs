using YurtBursu.Api.DTOs;

namespace YurtBursu.Api.Services
{
	public interface IBursService
	{
		Task<(BursResponse Response, int[] NormalizedExcludedDays)> CalculateBursAsync(BursRequest request, CancellationToken cancellationToken = default);
		Task<BursResponse> SaveHistoryAsync(BursRequest request, CancellationToken cancellationToken = default);
		Task<List<BursHistoryResponseDto>> GetHistoryAsync(int studentId, CancellationToken cancellationToken = default);
	}
}


