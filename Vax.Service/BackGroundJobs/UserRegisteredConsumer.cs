using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.BackGroundJobs
{
	public class UserRegisteredConsumer : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IConfiguration _configuration;

		public UserRegisteredConsumer(
			IServiceScopeFactory scopeFactory,
			IConfiguration configuration)
		{
			_scopeFactory = scopeFactory;
			_configuration = configuration;
		}

		protected async override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var factory = new ConnectionFactory
			{
				HostName = _configuration["RabbitMq:Host"]
			};

			var connection = await factory.CreateConnectionAsync();
			var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
				queue: "user.queue",
				durable: true,
				exclusive: false,
				autoDelete: false);


			var consumer = new AsyncEventingBasicConsumer(channel);

			consumer.ReceivedAsync += async (sender, args) =>
			{
				var body = args.Body.ToArray();

				var json = Encoding.UTF8.GetString(body);

				var message = System.Text.Json.JsonSerializer.Deserialize<UserRegisteredEvent>(json);
				using var scope = _scopeFactory.CreateScope();

				var emailService =
					scope.ServiceProvider.GetRequiredService<IEmailService>();


				emailService.SendEmail(new EmailDto
				{
					To = message.Email,
					Subject = "Welcome to Vax!",
					Body = $"Hello {message.FullName}, welcome to Vax!"
				});

				await channel.BasicAckAsync(
			   args.DeliveryTag,
			   false);
			};
			await channel.BasicConsumeAsync(
			queue: "user.queue",
			autoAck: false,
			consumer: consumer);

			await Task.Delay(
				Timeout.Infinite,
				stoppingToken);
		}

	}
}
