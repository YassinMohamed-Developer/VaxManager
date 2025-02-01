using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.SignalR
{
	public class NotificationHub : Hub
	{
		private readonly ILogger<NotificationHub> _logger;

		public NotificationHub(ILogger<NotificationHub> logger)
		{
			_logger = logger;
		}
		public async Task SendNotification(string message)
		{

			await Clients.All.SendAsync("ReceiveMessage",message);
			_logger.LogInformation(message);
		}
	}
}
