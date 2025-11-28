using Microsoft.AspNetCore.Mvc;
using YurtBursu.Api.DTOs;
using YurtBursu.Api.Services;

namespace YurtBursu.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BursController : ControllerBase
	{
		private readonly IBursService _bursService;

		public BursController(IBursService bursService)
		{
			_bursService = bursService;
		}

		[HttpPost("hesapla")]
		public async Task<ActionResult<BursResponse>> Hesapla([FromBody] BursRequest request, CancellationToken cancellationToken)
		{
			var (response, _) = await _bursService.CalculateBursAsync(request, cancellationToken);
			return Ok(response);
		}

		[HttpPost("history/save")]
		public async Task<ActionResult<BursResponse>> SaveHistory([FromBody] BursRequest request, CancellationToken cancellationToken)
		{
			var result = await _bursService.SaveHistoryAsync(request, cancellationToken);
			return Ok(result);
		}

		[HttpGet("history/{studentId:int}")]
		public async Task<ActionResult<List<BursHistoryResponseDto>>> GetHistory([FromRoute] int studentId, CancellationToken cancellationToken)
		{
			var result = await _bursService.GetHistoryAsync(studentId, cancellationToken);
			return Ok(result);
		}
	}
}


