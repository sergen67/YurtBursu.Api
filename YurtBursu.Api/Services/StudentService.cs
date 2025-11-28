using System.ComponentModel.DataAnnotations;
using YurtBursu.Api.DTOs;
using YurtBursu.Api.Models;
using YurtBursu.Api.Repositories;

namespace YurtBursu.Api.Services
{
	public class StudentService : IStudentService
	{
		private readonly IStudentRepository _studentRepository;

		public StudentService(IStudentRepository studentRepository)
		{
			_studentRepository = studentRepository;
		}

		public async Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto dto, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(dto.FullName))
			{
				throw new ValidationException("FullName is required.");
			}
			if (string.IsNullOrWhiteSpace(dto.Email))
			{
				throw new ValidationException("Email is required.");
			}
			if (!new EmailAddressAttribute().IsValid(dto.Email))
			{
				throw new ValidationException("Email is invalid.");
			}

			var existing = await _studentRepository.GetByEmailAsync(dto.Email, cancellationToken);
			if (existing is not null)
			{
				throw new ValidationException("A student with this email already exists.");
			}

			var student = new Student
			{
				FullName = dto.FullName.Trim(),
				Email = dto.Email.Trim(),
				CreatedAt = DateTime.UtcNow
			};

			var created = await _studentRepository.CreateAsync(student, cancellationToken);
			return new StudentResponseDto
			{
				Id = created.Id,
				FullName = created.FullName,
				Email = created.Email,
				CreatedAt = created.CreatedAt
			};
		}

		public async Task<StudentResponseDto> GetStudentByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			if (id <= 0)
			{
				throw new ValidationException("Student id must be positive.");
			}

			var student = await _studentRepository.GetByIdAsync(id, cancellationToken);
			if (student is null)
			{
				throw new KeyNotFoundException("Student not found.");
			}
			return new StudentResponseDto
			{
				Id = student.Id,
				FullName = student.FullName,
				Email = student.Email,
				CreatedAt = student.CreatedAt
			};
		}

		public async Task<List<StudentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var students = await _studentRepository.GetAllAsync(cancellationToken);
			return students.Select(s => new StudentResponseDto
			{
				Id = s.Id,
				FullName = s.FullName,
				Email = s.Email,
				CreatedAt = s.CreatedAt
			}).ToList();
		}
	}
}


