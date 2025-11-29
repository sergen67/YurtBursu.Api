namespace YurtBursu.Api.Services
{
	public interface INotificationService
	{
		Task SaveTokenAsync(int studentId, string token, CancellationToken cancellationToken = default);
		Task<int> SendNotificationToAllAsync(string title, string body, CancellationToken cancellationToken = default);
	}
}
