using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using YurtBursu.Api.DTOs;
using YurtBursu.Api.Models;
using YurtBursu.Api.Repositories;

namespace YurtBursu.Api.Services
{
	public class BursService : IBursService
	{
		private const int DailyRate = 200;
		private readonly IStudentRepository _studentRepository;
		private readonly IBursRepository _bursRepository;

		public BursService(IStudentRepository studentRepository, IBursRepository bursRepository)
		{
			_studentRepository = studentRepository;
			_bursRepository = bursRepository;
		}

		public async Task<(BursResponse Response, int[] NormalizedExcludedDays)> CalculateBursAsync(BursRequest request, CancellationToken cancellationToken = default)
		{
			await EnsureStudentExists(request.StudentId, cancellationToken);

			ValidateYearMonth(request.Year, request.Month);
			var totalDays = DateTime.DaysInMonth(request.Year, request.Month);
			var validExcluded = ValidateExcludedDays(request.ExcludedDays, totalDays);
			var excludedCount = validExcluded.Length;

			// Additional validation rule
			if (excludedCount > totalDays)
			{
				throw new ValidationException("Excluded days count cannot exceed total days in month.");
			}

			var calculated = (totalDays - excludedCount) * DailyRate;

			var response = new BursResponse
			{
				StudentId = request.StudentId,
				TotalDays = totalDays,
				ExcludedCount = excludedCount,
				Rate = DailyRate,
				CalculatedBurs = calculated
			};

			return (response, validExcluded);
		}

		public async Task<BursResponse> SaveHistoryAsync(BursRequest request, CancellationToken cancellationToken = default)
		{
			var (response, normalizedExcluded) = await CalculateBursAsync(request, cancellationToken);

			var excludedJson = JsonSerializer.Serialize(normalizedExcluded);

			var history = new BursHistory
			{
				StudentId = request.StudentId,
				Year = request.Year,
				Month = request.Month,
				ExcludedDays = excludedJson,
				CalculatedBurs = response.CalculatedBurs,
				CreatedAt = DateTime.UtcNow
			};

			await _bursRepository.SaveOrOverwriteAsync(history, cancellationToken);
			return response;
		}

		public async Task<List<BursHistoryResponseDto>> GetHistoryAsync(int studentId, CancellationToken cancellationToken = default)
		{
			await EnsureStudentExists(studentId, cancellationToken);
			var list = await _bursRepository.GetHistoryByStudentAsync(studentId, cancellationToken);
			return list.Select(x => new BursHistoryResponseDto
			{
				Id = x.Id,
				StudentId = x.StudentId,
				Year = x.Year,
				Month = x.Month,
				ExcludedDays = JsonSerializer.Deserialize<int[]>(x.ExcludedDays) ?? Array.Empty<int>(),
				CalculatedBurs = x.CalculatedBurs,
				CreatedAt = x.CreatedAt
			}).ToList();
		}

		private async Task EnsureStudentExists(int studentId, CancellationToken cancellationToken)
		{
			if (studentId <= 0)
			{
				throw new ValidationException("StudentId must be positive.");
			}
			var exists = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
			if (exists is null)
			{
				throw new KeyNotFoundException("Student not found.");
			}
		}

		private static void ValidateYearMonth(int year, int month)
		{
			var currentYear = DateTime.UtcNow.Year;
			if (year < 2000 || year > currentYear + 1)
			{
				throw new ValidationException($"Year must be between 2000 and {currentYear + 1}.");
			}
			if (month < 1 || month > 12)
			{
				throw new ValidationException("Month must be between 1 and 12.");
			}
		}

		private static int[] ValidateExcludedDays(int[]? excludedDays, int totalDays)
		{
			if (excludedDays is null || excludedDays.Length == 0)
			{
				return Array.Empty<int>();
			}

			// Ensure in-range and sorted ascending with no duplicates
			for (var i = 0; i < excludedDays.Length; i++)
			{
				var d = excludedDays[i];
				if (d < 1 || d > totalDays)
				{
					throw new ValidationException($"Excluded day {d} is out of range 1..{totalDays}.");
				}
				if (i > 0 && excludedDays[i - 1] > d)
				{
					throw new ValidationException("ExcludedDays must be sorted ascending.");
				}
			}

			// Check uniqueness
			for (var i = 1; i < excludedDays.Length; i++)
			{
				if (excludedDays[i] == excludedDays[i - 1])
				{
					throw new ValidationException("ExcludedDays must not contain duplicates.");
				}
			}

			return excludedDays;
		}
	}
}


