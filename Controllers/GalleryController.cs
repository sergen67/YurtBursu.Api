using Microsoft.AspNetCore.Mvc;
using YurtBursu.Api.Models;
using YurtBursu.Api.Services;

namespace YurtBursu.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GalleryController : ControllerBase
	{
		private readonly IGalleryService _galleryService;

		public GalleryController(IGalleryService galleryService)
		{
			_galleryService = galleryService;
		}

		/// <summary>
		/// Uploads an image file to the gallery. Protected by BasicAuth middleware.
		/// </summary>
		[HttpPost("upload")]
		public async Task<ActionResult<GalleryItem>> Upload([FromForm] IFormFile file, CancellationToken cancellationToken)
		{
			var item = await _galleryService.UploadImageAsync(file, cancellationToken);
			return Ok(item);
		}

		/// <summary>
		/// Lists all gallery items (public).
		/// </summary>
		[HttpGet("list")]
		public async Task<ActionResult<List<GalleryItem>>> List(CancellationToken cancellationToken)
		{
			var items = await _galleryService.ListAsync(cancellationToken);
			return Ok(items);
		}

	}
}


