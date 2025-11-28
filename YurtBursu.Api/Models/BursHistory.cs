using System;

namespace YurtBursu.Api.Models
{
	public class BursHistory
	{
		public int Id { get; set; }
		public int StudentId { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public string ExcludedDays { get; set; } = "[]"; // JSON string
		public int CalculatedBurs { get; set; }
		public DateTime CreatedAt { get; set; }

		public Student Student { get; set; } = null!;
	}
}


