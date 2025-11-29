using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using YurtBursu.Api.Data;
using YurtBursu.Api.Middleware;
using YurtBursu.Api.Repositories;
using YurtBursu.Api.Services;
using YurtBursu.Api.Swagger;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperationFilter>();
    c.CustomSchemaIds(type => type.ToString());

    // Basic Auth Definition
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

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

app.UseStaticFiles(); // Enable static file serving for uploads

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("DefaultCors");
app.UseRateLimiter();
app.UseMiddleware<BasicAuthMiddleware>();
// app.UseHttpsRedirection(); // not forced for Render

app.UseAuthorization();

app.MapControllers();

app.Run();
