using System.ComponentModel.DataAnnotations;
using YurtBursu.Api.Models;
using YurtBursu.Api.Repositories;

namespace YurtBursu.Api.Services
{
	public class GalleryService : IGalleryService
	{
		private readonly IGalleryRepository _repo;
		public GalleryService(IGalleryRepository repo)
		{
			_repo = repo;
		}

		public async Task<GalleryItem> AddAsync(string url, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ValidationException("ImageUrl is required.");
			}
			if (!Uri.TryCreate(url, UriKind.Absolute, out _))
			{
				throw new ValidationException("ImageUrl must be a valid absolute URL.");
			}
			return await _repo.AddAsync(url, cancellationToken);
		}

		public Task<List<GalleryItem>> ListAsync(CancellationToken cancellationToken = default)
		{
			return _repo.ListAsync(cancellationToken);
		}
	}
}


