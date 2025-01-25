using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.Helper
{
	public class CustomException : Exception
	{
		public CustomException() : base() { }

		public CustomException(string message) : base(message) { }

		public CustomException(string message, params object[] args)
			: base(String.Format(CultureInfo.CurrentCulture, message, args))
		{
		}
		public int StatusCode { get; set; }


	}
}
