using System;

namespace YurtBursu.Api.Models
{
	public class GalleryItem
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}


