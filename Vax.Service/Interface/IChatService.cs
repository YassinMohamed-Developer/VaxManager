using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Helper;

namespace Vax.Service.Interface
{
	public interface IChatService
	{
		public Task<BaseResult<string>> Ask(ChatRequest request, string appuserId);
	}
}
