using eCommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace eCommerceApp.Infrastructure.Persistence
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			//seed data(hazır kayıtları oluşturup veritabanına göndereceğiz)
			var hasher = new PasswordHasher<AppUser>();
			var now = DateTime.UtcNow;

			// Seed Roles
			var adminRole = new IdentityRole
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Admin",
				NormalizedName = "ADMIN"
			};
			var userRole = new IdentityRole
			{
				Id = Guid.NewGuid().ToString(),
				Name = "User",
				NormalizedName = "USER"
			};

			builder.Entity<IdentityRole>().HasData(adminRole, userRole);

			// Seed Users
			var adminUser = new AppUser
			{
				Id = Guid.NewGuid().ToString(),
				Fullname = "Admin User",
				UserName = "admin@example.com",
				NormalizedUserName = "ADMIN@EXAMPLE.COM",
				Email = "admin@example.com",
				NormalizedEmail = "ADMIN@EXAMPLE.COM",
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString(),
				IsActive = true,
				LastLoginDate = now,
				RegistrationDate = now,
				CreatedDate = now,
				ModifiedDate = now,
				IsDeleted = false
			};
			adminUser.PasswordHash = hasher.HashPassword(adminUser, "AdminPassword123!");

			var regularUser = new AppUser
			{
				Id = Guid.NewGuid().ToString(),
				Fullname = "Regular User",
				UserName = "user@example.com",
				NormalizedUserName = "USER@EXAMPLE.COM",
				Email = "user@example.com",
				NormalizedEmail = "USER@EXAMPLE.COM",
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString(),
				IsActive = true,
				LastLoginDate = now,
				RegistrationDate = now,
				CreatedDate = now,
				ModifiedDate = now,
				IsDeleted = false
			};
			regularUser.PasswordHash = hasher.HashPassword(regularUser, "UserPassword123!");

			builder.Entity<AppUser>().HasData(adminUser, regularUser);

			// Assign Roles to Users
			builder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>
				{
					UserId = adminUser.Id,
					RoleId = adminRole.Id
				},
				new IdentityUserRole<string>
				{
					UserId = regularUser.Id,
					RoleId = userRole.Id
				}
			);
		}
	}
}