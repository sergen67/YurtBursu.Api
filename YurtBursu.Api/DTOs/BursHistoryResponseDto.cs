using System;

namespace YurtBursu.Api.DTOs
{
	public class BursHistoryResponseDto
	{
		public int Id { get; set; }
		public int StudentId { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int[] ExcludedDays { get; set; } = Array.Empty<int>();
		public int CalculatedBurs { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}


