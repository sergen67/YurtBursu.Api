using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public interface IStudentRepository
	{
		Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
		Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
		Task<Student> CreateAsync(Student student, CancellationToken cancellationToken = default);
		Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default);
	}
}


