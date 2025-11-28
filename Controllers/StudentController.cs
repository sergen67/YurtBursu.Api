using Microsoft.AspNetCore.Mvc;
using YurtBursu.Api.DTOs;
using YurtBursu.Api.Services;

namespace YurtBursu.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StudentController : ControllerBase
	{
		private readonly IStudentService _studentService;

		public StudentController(IStudentService studentService)
		{
			_studentService = studentService;
		}

		[HttpPost("create")]
		public async Task<ActionResult<StudentResponseDto>> Create([FromBody] StudentCreateDto dto, CancellationToken cancellationToken)
		{
			var created = await _studentService.CreateStudentAsync(dto, cancellationToken);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<StudentResponseDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
		{
			var student = await _studentService.GetStudentByIdAsync(id, cancellationToken);
			return Ok(student);
		}

		[HttpGet("all")]
		public async Task<ActionResult<List<StudentResponseDto>>> GetAll(CancellationToken cancellationToken)
		{
			var students = await _studentService.GetAllAsync(cancellationToken);
			return Ok(students);
		}
	}
}


