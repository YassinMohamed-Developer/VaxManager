﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.RequestDto
{
	public class EmailDto
	{

			public string Subject { get; set; }

			public string Body { get; set; }

			public string From { get; set; }

			public string To { get; set; }
	}
}
