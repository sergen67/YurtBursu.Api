using YurtBursu.Api.Models;

namespace YurtBursu.Api.Services
{
	public interface IGalleryService
	{
		Task<GalleryItem> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default);
		Task<List<GalleryItem>> ListAsync(CancellationToken cancellationToken = default);
	}
}


