using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Configuration;

namespace ParkingService.Services
{
    public class SmsService
    {
        string accountSid = "AC2289cce109244962ff3e67ed4e0408be";
        string authToken = "483b3400345fcca92c7916aa90dc6876";

        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration.GetValue<string>("Twilio:AccountSid");
            _authToken = configuration.GetValue<string>("Twilio:AuthToken");
            _fromPhoneNumber = configuration.GetValue<string>("Twilio:FromPhoneNumber");
        }

        public void SendSms(string toPhoneNumber, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(toPhoneNumber))
            {
                From = new PhoneNumber(_fromPhoneNumber),
                Body = message
            };

            var messageResource = MessageResource.Create(messageOptions);
            Console.WriteLine($"SMS sent with SID: {messageResource.Sid}");
        }
    }
}
