using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.Helper
{
	public class BaseResult<T>
	{
		public bool IsSuccess { get; set; } = true;

		public string Message { get; set; } = string.Empty;
		public T Data { get; set; }

		public List<string> Errors { get; set; }
	}
}
