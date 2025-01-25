using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vax.Data.Context;
using Vax.Data.Entity;
using Vax.Data.Seeding;
using Vax.Repository.Interface;

namespace VaxManager.Helper
{
	public class ApplySeeding
	{
		public static async Task ApplySeedingAsync(WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var service = scope.ServiceProvider;

				try
				{
					var context = service.GetRequiredService<Vaxdbcontext>();

					var rolemanager = service.GetRequiredService<RoleManager<IdentityRole>>();

					var usermanager = service.GetRequiredService<UserManager<AppUser>>();

					var unitofwork = service.GetRequiredService<IUnitOfWork>();

					await context.Database.MigrateAsync();

					await SeedRoleContext.SeedRoleAsync(rolemanager,usermanager,unitofwork);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}
