using Microsoft.AspNetCore.Identity;
using Pinterest.Entities;

namespace Pinterest.Helper
{
	public static class DataSeed
	{
		public static async Task InitializeAsync(IServiceProvider serviceProvider)
		{

			using (var scope = serviceProvider.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				string[] roles = new[] { "Admin" };

				foreach (string role in roles)
				{
					var exists = await roleManager.RoleExistsAsync(role);
					if (exists) continue;
					await roleManager.CreateAsync(new IdentityRole(role));
				}

				var user = new AppUser
				{
					FirstName = "admin",
					LastName = "admin",
					Email = "admin@mail.com",
					UserName = "admin",
				};

				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
				var existingUser = await userManager.FindByNameAsync("Admin");
				if (existingUser is not null) return;

				await userManager.CreateAsync(user, "Admin12345");
				await userManager.AddToRoleAsync(user, roles[0]);

				return;
			}
		}
	}
}
