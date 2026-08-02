using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.Helper;

namespace Vax.Service.Interface
{
	public interface IRabbitMqPublisher
	{
		Task PublishAsync(UserRegisteredEvent message);
	}
}
