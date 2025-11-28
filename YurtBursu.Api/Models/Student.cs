using System;

namespace YurtBursu.Api.Models
{
	public class Student
	{
		public int Id { get; set; }
		public string FullName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public List<BursHistory> Histories { get; set; } = new();
	}
}


