using Microsoft.OpenApi.Models;

namespace VaxManager.Extension
{
	public static class SwaggerServiceExtension
	{
		public static void SwaggerService(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc(
					"v1",
					new OpenApiInfo
					{
						Title = "VaxManagerApi",
						Version = "v1",
					});
				var securityScheme = new OpenApiSecurityScheme
				{
					Description = "JWT Autho",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "bearer",
					Reference = new OpenApiReference
					{
						Id = "bearer",
						Type = ReferenceType.SecurityScheme
					}
				};
				options.AddSecurityDefinition("bearer", securityScheme);

				var securityRequirements = new OpenApiSecurityRequirement
				{
					{securityScheme,new[]{ "bearer"} }
				};

				options.AddSecurityRequirement(securityRequirements);
			});
		}
	}
}
