using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class TestService : ITestService
	{
		public BaseResult<string> GetById(int Id)
		{
			if (Id == 0)
			{
				throw new CustomException("Id IS zero") { StatusCode = (int)HttpStatusCode.BadRequest };
			}

			return new BaseResult<string> { Data = Id.ToString(),IsSuccess = true, Message = "Data Retrieve Sucess" };
		}
	}
}
