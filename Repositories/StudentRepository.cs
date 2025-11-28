using Microsoft.EntityFrameworkCore;
using YurtBursu.Api.Data;
using YurtBursu.Api.Models;

namespace YurtBursu.Api.Repositories
{
	public class StudentRepository : IStudentRepository
	{
		private readonly AppDbContext _dbContext;

		public StudentRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Students
				.AsNoTracking()
				.Include(s => s.Histories)
				.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
		}

		public async Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
		}

		public async Task<Student> CreateAsync(Student student, CancellationToken cancellationToken = default)
		{
			await _dbContext.Students.AddAsync(student, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return student;
		}

		public async Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _dbContext.Students.AsNoTracking().OrderBy(s => s.Id).ToListAsync(cancellationToken);
		}
	}
}


