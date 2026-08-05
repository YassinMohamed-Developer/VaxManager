
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.SemanticKernel;
using Org.BouncyCastle.Asn1.Cms;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Threading.RateLimiting;
using Twilio.Types;
using Vax.Data.Context;
using Vax.Data.Entity;
using Vax.Repository.Implmentation;
using Vax.Repository.Interface;
using Vax.Service.BackGroundJob;
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
			builder.Services.AddMemoryCache();



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
			//builder.Services.AddScoped<DailyEmailMessageJob>();


				builder.Services.AddHangfire(configuration =>
				{
					configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
				});
			builder.Services.AddHangfireServer();

			builder.Services.AddRateLimiter(options =>
			{
				options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

				options.AddFixedWindowLimiter("Fixed", x =>
				{
					x.PermitLimit = 10;
					x.Window = TimeSpan.FromSeconds(20);
					x.QueueLimit = 0;
					x.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
				});

			});

			builder.Services.AddRateLimiter(options =>
			{
				options.AddTokenBucketLimiter("Token", x =>
				{
					x.TokenLimit = 10;
					x.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
					x.TokensPerPeriod = 2;
				});
				options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
			});

			builder.Services.AddRateLimiter(options =>
			{

				options.AddPolicy("fixedIpAddress", httpContext =>
				RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
				factory:_ => new FixedWindowRateLimiterOptions
				{
					PermitLimit = 10,
					Window = TimeSpan.FromSeconds(10),
					QueueLimit = 0,
					QueueProcessingOrder = QueueProcessingOrder.OldestFirst
				}));
				options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
			});


			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
				.MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
				.WriteTo.MongoDB(builder.Configuration.GetConnectionString("MongoDBConnection"), "Logs")
				.WriteTo.Console()
				.CreateLogger();

			builder.Host.UseSerilog();


			#endregion

			var app = builder.Build();
			await ApplySeeding.ApplySeedingAsync(app);
			app.UseMiddleware<CustomExceptionHandlerMiddleware>();
			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
				app.UseSwagger();
				app.UseSwaggerUI();
			//}


			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseHangfireDashboard("/hangfire");
			app.UseRateLimiter();

			//RecurringJob.AddOrUpdate<DailyEmailMessageJob>
			//	(
			//		"DailyEmailMessageJob",
			//		(job) => job.SendEmail(),
			//		Cron.Never
			//	);
			app.UseCors("default");

			app.MapControllers();

			app.UseHealthChecks("/api/Health/CheckDatabaseHealth", new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
			});

			app.MapHub<NotificationHub>("/NotificationHub");
			app.Run();
		}
	}
}
