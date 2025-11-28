namespace YurtBursu.Api.DTOs
{
	public class BursRequest
	{
		public int StudentId { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int[] ExcludedDays { get; set; } = [];
	}
}


