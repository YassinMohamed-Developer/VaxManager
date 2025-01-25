using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Entity;
using Vax.Service.Interface;
using VaxManager.Helper;

namespace Vax.Service.Implmentation
{
	public class TokenService : ITokenService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IOptions<TokenOption> _options;
		private readonly SymmetricSecurityKey _Key;
		public TokenService(UserManager<AppUser> userManager,IOptions<TokenOption> options)
        {
			_userManager = userManager;
			_options = options;
			_Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
		}
        public async Task<string> GenerateToken(AppUser appUser)
		{

			var roles = await _userManager.GetRolesAsync(appUser);
			var claims = new List<Claim>
			{
				new Claim("UserId",appUser.Id),
				new Claim("UserName",appUser.UserName),
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			//Hashing The Key
			var credential = new SigningCredentials(_Key,SecurityAlgorithms.HmacSha256);


			//Formation Your Token
			var TokenDescribe = new SecurityTokenDescriptor
			{
				SigningCredentials = credential,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddHours(1),
				Issuer = _options.Value.Issuer,
				IssuedAt = DateTime.Now,
			};

			var handler = new JwtSecurityTokenHandler();

			var token = handler.CreateToken(TokenDescribe);

			return handler.WriteToken(token);
		}
	}
}
