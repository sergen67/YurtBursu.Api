using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public interface IGalleryRepository
	{
		Task<GalleryItem> AddAsync(string url, CancellationToken cancellationToken = default);
		Task<List<GalleryItem>> ListAsync(CancellationToken cancellationToken = default);
	}
}


