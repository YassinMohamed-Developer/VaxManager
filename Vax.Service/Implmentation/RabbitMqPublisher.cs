using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class RabbitMqPublisher : IRabbitMqPublisher
	{
		private readonly IConfiguration _configuration;

		public RabbitMqPublisher(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task PublishAsync(UserRegisteredEvent message)
		{
			var factory = new ConnectionFactory()
			{
				HostName = _configuration["RabbitMQ:Host"]
			};

			using var connection = await factory.CreateConnectionAsync();

			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(
			queue: "user.queue",
			durable: true,
			exclusive: false,
			autoDelete: false);


			await channel.ExchangeDeclareAsync(
				exchange: "user.exchange",
				type: ExchangeType.Direct,
				autoDelete: false,
				durable:true);

			await channel.QueueBindAsync(
				exchange: "user.exchange",
				queue: "user.queue",
				routingKey: "user-registered"
				);

			var body = Encoding.UTF8.GetBytes(
		   JsonSerializer.Serialize(message));


			await channel.BasicPublishAsync(
			exchange: "user.exchange",
			routingKey: "user-registered",
			body: body);
		}
	}
}
