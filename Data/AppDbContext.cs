using Microsoft.EntityFrameworkCore;
using YurtBursu.Api.Models;

namespace YurtBursu.Api.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Student> Students => Set<Student>();
		public DbSet<BursHistory> BursHistories => Set<BursHistory>();
		public DbSet<GalleryItem> Gallery => Set<GalleryItem>();
		public DbSet<NotificationToken> NotificationTokens => Set<NotificationToken>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Student>(entity =>
			{
				entity.ToTable("Students");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
				entity.HasIndex(e => e.Email).IsUnique();
				entity.Property(e => e.CreatedAt).IsRequired();
			});

			modelBuilder.Entity<BursHistory>(entity =>
			{
				entity.ToTable("BursHistories");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Year).IsRequired();
				entity.Property(e => e.Month).IsRequired();
				entity.Property(e => e.ExcludedDays).IsRequired();
				entity.Property(e => e.CalculatedBurs).IsRequired();
				entity.Property(e => e.CreatedAt).IsRequired();
				entity.HasOne(b => b.Student)
					.WithMany(s => s.Histories)
					.HasForeignKey(e => e.StudentId)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasIndex(e => new { e.StudentId, e.Year, e.Month }).IsUnique();
			});

			modelBuilder.Entity<GalleryItem>(entity =>
			{
				entity.ToTable("Gallery");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(2048);
				entity.Property(e => e.CreatedAt).IsRequired();
			});

			modelBuilder.Entity<NotificationToken>(entity =>
			{
				entity.ToTable("NotificationTokens");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Token).IsRequired().HasMaxLength(4096);
				entity.Property(e => e.CreatedAt).IsRequired();
				entity.HasIndex(e => e.Token).IsUnique(false);
				entity.HasOne<Student>()
					.WithMany()
					.HasForeignKey(e => e.StudentId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}


