using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Service.Helper;

namespace Vax.Service.Interface
{
	public interface ITestService
	{
		public BaseResult<string> GetById(int Id);
	}
}
