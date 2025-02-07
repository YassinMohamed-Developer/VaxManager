using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vax.Data.Context;
using Vax.Data.Entity;

namespace VaxManager.Extension
{
	public static class IdentityServiceExtension
	{
		public static void IdentityService(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddIdentityCore<AppUser>().AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<Vaxdbcontext>()
			.AddSignInManager<SignInManager<AppUser>>()
			.AddRoleManager<RoleManager<IdentityRole>>()
			.AddDefaultTokenProviders();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(option =>

				   option.TokenValidationParameters = new TokenValidationParameters
				   {
					   ValidateIssuerSigningKey = true,
					   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
					   ValidateIssuer = true,
					   ValidIssuer = configuration["Token:Issuer"],
					   ValidateAudience = false,
					   ValidateLifetime = true,
				   }
				);
		}
	}
}
