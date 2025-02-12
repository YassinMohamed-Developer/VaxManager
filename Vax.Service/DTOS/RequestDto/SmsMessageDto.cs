using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Service.DTOS.RequestDto
{
    public class SmsMessageDto
    {
        public string PhoneNumber { get; set; }

        public string Body { get; set; }


    }
}
