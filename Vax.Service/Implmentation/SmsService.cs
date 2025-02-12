using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
    public class SmsService : ISmsService
    {
        private TwilioOption _options;

        public SmsService(IOptions<TwilioOption> options)
        {
            _options = options.Value;
        }
        public MessageResource Send(SmsMessageDto sms)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);

            var Message = MessageResource.Create(
                body:sms.Body,
                from:new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
                to:sms.PhoneNumber
            );

            return Message;
        }
    }
}
