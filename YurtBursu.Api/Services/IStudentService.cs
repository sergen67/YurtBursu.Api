using YurtBursu.Api.DTOs;

namespace YurtBursu.Api.Services
{
	public interface IStudentService
	{
		Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto dto, CancellationToken cancellationToken = default);
		Task<StudentResponseDto> GetStudentByIdAsync(int id, CancellationToken cancellationToken = default);
		Task<List<StudentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
	}
}


