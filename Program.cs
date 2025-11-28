using Microsoft.EntityFrameworkCore;
using YurtBursu.Api.Data;
using YurtBursu.Api.Middleware;
using YurtBursu.Api.Repositories;
using YurtBursu.Api.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
	options.AddPolicy("DefaultCors", policy =>
	{
		policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
	});
});
// DbContext - MSSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(connectionString));

// DI for Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IBursRepository, BursRepository>();
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// DI for Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IBursService, BursService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Rate limiting
builder.Services.AddRateLimiter(_ => _
	.AddFixedWindowLimiter("fixed", options =>
	{
		options.PermitLimit = 100;
		options.Window = TimeSpan.FromMinutes(1);
		options.QueueLimit = 0;
	}));


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("DefaultCors");
app.UseRateLimiter();
app.UseMiddleware<BasicAuthMiddleware>();
// app.UseHttpsRedirection(); // not forced for Render

app.UseAuthorization();

app.MapControllers();

app.Run();


