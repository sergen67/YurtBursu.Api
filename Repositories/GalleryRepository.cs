using Microsoft.EntityFrameworkCore;
using YurtBursu.Api.Data;
using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public class GalleryRepository : IGalleryRepository
	{
		private readonly AppDbContext _db;
		public GalleryRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task<GalleryItem> AddAsync(string url, CancellationToken cancellationToken = default)
		{
			var entity = new GalleryItem { ImageUrl = url, CreatedAt = DateTime.UtcNow };
			await _db.Gallery.AddAsync(entity, cancellationToken);
			await _db.SaveChangesAsync(cancellationToken);
			return entity;
		}

		public async Task<List<GalleryItem>> ListAsync(CancellationToken cancellationToken = default)
		{
			return await _db.Gallery.AsNoTracking().OrderByDescending(g => g.CreatedAt).ToListAsync(cancellationToken);
		}
	}
}


