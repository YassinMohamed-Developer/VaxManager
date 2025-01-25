using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Repository.Interface;

namespace Vax.Data.Seeding
{
	//DOTO Make HardCoded for Admin Table
	public class SeedRoleContext
	{
		public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager,UserManager<AppUser> _userManager,IUnitOfWork _unitOfWork)
		{
			if (!roleManager.Roles.Any())
			{
				List<IdentityRole> roles = [new IdentityRole() { Name = "Admin" }, new IdentityRole { Name = "Patient" },new IdentityRole { Name = "VaccineCenter" }];
				{

				}
				foreach (var role in roles)
				{
					await roleManager.CreateAsync(role);
				}
			}

			var adminemails = new[] { "ym9807770@gmail.com", "yassinmohamed.cs@gmail.com" };

			foreach (var adminemail in adminemails)
			{
				var adminuser = await _userManager.FindByEmailAsync(adminemail);

				if(adminuser is null)
				{
					var appuser = new AppUser
					{
						Email = adminemail,
						UserName = adminemail.Split('@')[0],
					};
					await _userManager.CreateAsync(appuser,"Y123@a");
					await _userManager.AddToRoleAsync(appuser, "Admin");

					var admin = new Admin
					{
						Email = adminemail,
						AppUserId = appuser.Id,
						FullName = appuser.UserName,
					};

					await _unitOfWork.Admins.AddAsync(admin);
					_unitOfWork.Complete();
				}
			}
		}
	}
}
