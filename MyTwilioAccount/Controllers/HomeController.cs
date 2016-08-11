using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace MyTwilioAccount.Controllers
{
    public class HomeController : TwilioController //Controller
    {
        // GET: Home
        [HttpPost]
        public ActionResult Index(Twilio.Mvc.VoiceRequest request)
        {
            //return View();
            return Incomming(request);
        }

        private object RetrieveVoiceLanguage(Twilio.Mvc.VoiceRequest request)
        {
            var voice_NO = new { voice = "alice", language = "nb-NO" };
            var voice_DK = new { voice = "alice", language = "da-DK" };
            var voice_SE = new { voice = "alice", language = "sv-SE" };
            var voice_EN = new { voice = "alice", language = "en-GB" };

            var myvoice = voice_EN;

            var country = request.FromCountry.ToString();
            switch (country.ToUpper())
            {
                case ("NO"):
                    myvoice = voice_NO;
                    break;
                case ("DK"):
                    myvoice = voice_DK;
                    break;
                case ("SE"):
                    myvoice = voice_SE;
                    break;
                case ("GB"):
                    myvoice = voice_EN;
                    break;
                default:
                    myvoice = voice_EN;
                    break;
            }

            return myvoice;
        }
        private string RetrieveTextLanguage(Twilio.Mvc.VoiceRequest request)
        {
            var string_NO = " Hei {0}.  Dette er en melding fra DiabetesGuard og fra SmartCare. Ha en fin dag!";
            var string_DK = " Hei {0}. Dette er en besked fra DiabetesGuard og fra SmartCare. Hav en fin dag!";
            var string_SE = " Hej {0}.  Detta är en meddelande från DiabetesGuard och från SmartCare. Ha en fin dag!";
            var string_EN = " Hello {0}. This is e message from DiabetesGuard and SmartCare. Have a pleasant day!";
            var mystring = "";

            var country = request.FromCountry.ToString();
            switch (country.ToUpper())
            {
                case ("NO"):
                    mystring = string_NO;
                    break;
                case ("DK"):
                    mystring = string_DK;
                    break;
                case ("SE"):
                    mystring = string_SE;
                    break;
                case ("GB"):
                    mystring = string_EN;
                    break;
                default:
                    mystring = "I do not reqognize where you are calling from!";
                    break;
            }

            return mystring;

        }

        [HttpPost]
        public ActionResult Incomming (Twilio.Mvc.VoiceRequest request)
        {
            var response = new TwilioResponse();

            var myvoice = this.RetrieveVoiceLanguage(request);
            var mystring = this.RetrieveTextLanguage(request);

            var callerName = request.CallerName;
            response.Say(string.Format(mystring, callerName), myvoice)
                .Pause(2)
              .Hangup ();

            return TwiML(response);
        }
    }
}