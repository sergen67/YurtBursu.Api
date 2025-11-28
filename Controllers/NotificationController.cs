using Microsoft.AspNetCore.Mvc;
using YurtBursu.Api.Services;
using System.ComponentModel.DataAnnotations;

namespace YurtBursu.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationService _notificationService;

		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		/// <summary>
		/// Registers a device token for a student (public endpoint).
		/// </summary>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				return BadRequest(new { error = "Request body is required." });
			}
			if (request.StudentId <= 0)
			{
				return BadRequest(new { error = "StudentId must be positive." });
			}
			if (string.IsNullOrWhiteSpace(request.Token))
			{
				return BadRequest(new { error = "Token is required." });
			}

			await _notificationService.SaveTokenAsync(request.StudentId, request.Token, cancellationToken);
			return Ok(new { success = true });
		}

		/// <summary>
		/// Sends a notification to all registered tokens (admin endpoint).
		/// </summary>
		[HttpPost("send")]
		public async Task<ActionResult<object>> Send([FromBody] SendRequest request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				return BadRequest(new { error = "Request body is required." });
			}
			if (string.IsNullOrWhiteSpace(request.Title))
			{
				return BadRequest(new { error = "Title is required." });
			}
			if (string.IsNullOrWhiteSpace(request.Body))
			{
				return BadRequest(new { error = "Body is required." });
			}
			var count = await _notificationService.SendNotificationToAllAsync(request.Title, request.Body, cancellationToken);
			return Ok(new { sent = count });
		}

		public class RegisterRequest
		{
			[Required]
			[Range(1, int.MaxValue)]
			public int StudentId { get; set; }
			[Required]
			public string Token { get; set; } = string.Empty;
		}
		public class SendRequest
		{
			[Required]
			public string Title { get; set; } = string.Empty;
			[Required]
			public string Body { get; set; } = string.Empty;
		}
	}
}


