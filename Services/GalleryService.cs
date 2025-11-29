using System.ComponentModel.DataAnnotations;
using YurtBursu.Api.Models;
using YurtBursu.Api.Repositories;

namespace YurtBursu.Api.Services
{
	public class GalleryService : IGalleryService
	{
		private readonly IGalleryRepository _repo;
		private readonly IWebHostEnvironment _env;

		public GalleryService(IGalleryRepository repo, IWebHostEnvironment env)
		{
			_repo = repo;
			_env = env;
		}

		public async Task<GalleryItem> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default)
		{
			if (file is null || file.Length == 0)
			{
				throw new ValidationException("File is required.");
			}

			// Ensure uploads directory exists
			var uploadsPath = Path.Combine(_env.WebRootPath ?? Directory.GetCurrentDirectory(), "wwwroot", "uploads");
			if (!Directory.Exists(uploadsPath))
			{
				Directory.CreateDirectory(uploadsPath);
			}

			// Generate unique filename
			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			var filePath = Path.Combine(uploadsPath, fileName);

			// Save file
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream, cancellationToken);
			}

			// Generate public URL (assuming backend is serving static files)
			// Note: In production, you should configure base URL
			var url = $"/uploads/{fileName}";

			return await _repo.AddAsync(url, cancellationToken);
		}

		public Task<List<GalleryItem>> ListAsync(CancellationToken cancellationToken = default)
		{
			return _repo.ListAsync(cancellationToken);
		}
	}
}


