
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Vax.Data.Context;
using Vax.Data.Entity;
using Vax.Repository.Implmentation;
using Vax.Repository.Interface;
using Vax.Service.Helper;
using Vax.Service.Implmentation;
using Vax.Service.Interface;
using Vax.Service.SignalR;
using VaxManager.Extension;
using VaxManager.Helper;
using VaxManager.Middlewares;
namespace VaxManager
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			#region Configure Service
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("Token"));
			builder.Services.Configure<MailSettingsOptions>(builder.Configuration.GetSection("MailSettings"));
			builder.Services.Configure<TwilioOption>(builder.Configuration.GetSection("Twilio"));

			builder.Services.AddHealthChecks().AddDbContextCheck<Vaxdbcontext>();
			builder.Services.AddDbContext<Vaxdbcontext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			builder.Services.AddSignalR();
			builder.Services.AddCors(o =>
			{
				o.AddPolicy("default", x =>
				{
					x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
				});
			});
			builder.Services.RegisterService();
			builder.Services.IdentityService(builder.Configuration);
			builder.Services.SwaggerService();

			#endregion 

			var app = builder.Build();
			await ApplySeeding.ApplySeedingAsync(app);
			app.UseMiddleware<CustomExceptionHandlerMiddleware>();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCors("default");

			app.MapControllers();

			app.MapHub<NotificationHub>("/NotificationHub");
			app.Run();
		}
	}
}
