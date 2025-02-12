using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Vax.Service.DTOS.RequestDto;

namespace Vax.Service.Interface
{
   public  interface ISmsService
   {
        public MessageResource Send(SmsMessageDto sms);
   }
}
