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
        #region[Fields]
        private List<object> _myVoices;
        private List<string> _myTexts;
        #endregion

        #region[properties]
        private object myVoice { get;  set; }
        private string myText { get; set; }

        private List<object> myVoices
        {
            get
            {
                return this._myVoices ?? (this._myVoices = this.VoiceLanguages());
            }
        }
        private List<string> myTexts
        {
            get
            {
                return this._myTexts ?? (this._myTexts = this.TextLanguages());
            }
        }

        private List<object> VoiceLanguages()
        {
            List<object> _mylist = new List<object>();
            _mylist.Add(new { voice = "alice", language = "nb-NO" });//0
            _mylist.Add(new { voice = "alice", language = "da-DK" });//1
            _mylist.Add(new { voice = "alice", language = "sv-SE" });//2
            _mylist.Add(new { voice = "alice", language = "en-GB" });//3

            return _mylist;
        }

        private List<string> TextLanguages()
        {
            var string_NO = " Hei! Dette er en melding fra DiabetesGuard.";
            var string_DK = " Hei! Dette er en besked fra DiabetesGuard.";
            var string_SE = " Hej! Detta är en meddelande från DiabetesGuard.";
            var string_GB = " Hello! This is e message from DiabetesGuard.";

            List<string> _mylist = new List<string>();

            _mylist.Add(string_NO);//0
            _mylist.Add(string_DK);//1
            _mylist.Add(string_SE);//2
            _mylist.Add(string_GB);//3
        
            return _mylist;
        }
        #endregion

        #region[Methods]


        private void InitVoiceLanguage(Twilio.Mvc.VoiceRequest request)
        {
            myVoice = myVoices[3];

            if (request.FromCountry == null)
                return;

            var country = request.FromCountry.ToString();
            switch (country.ToUpper())
            {
                case ("NO"):
                    myVoice = myVoices[0];
                    break;
                case ("DK"):
                    myVoice = myVoices[1];
                    break;
                case ("SE"):
                    myVoice = myVoices[2];
                    break;
                case ("GB"):
                default:
                    myVoice = myVoices[3];
                    break;
            }
        }

        private void InitTextLanguage(Twilio.Mvc.VoiceRequest request)
        {
            myText = myTexts[3];

            if (request.FromCountry == null)
                return;

            var country = request.FromCountry.ToString();
            switch (country.ToUpper())
            {
                case ("NO"):
                    myText = myTexts[0];
                    break;
                case ("DK"):
                    myText = myTexts[1];
                    break;
                case ("SE"):
                    myText = myTexts[2];
                    break;
                case ("GB"):
                default:
                    myText = myTexts[3];
                    break;
            }
        }


        public ActionResult ViewHome()
        {
            return View();
        }
        #endregion

        [HttpPost]
        public ActionResult Incomming (Twilio.Mvc.VoiceRequest request)
        {
            var response = new TwilioResponse();

            this.InitVoiceLanguage(request);
            this.InitTextLanguage(request);

            myVoice = new { voice = "alice", language = "en-GB" };
            myText = "Hi!  this is an automatic message from my twilio test environment.";        
                
            response.Say(myText, myVoice)
                .Pause(1);
            

            response.Say("Please press a number between 1 and 3", myVoice).
                BeginGather(new { numDigit = "1", method = "GET", action = "http://mytwilioaccount.azurewebsites.net/home/test", timeout = 5})
                .EndGather();

            return TwiML(response);
        }

        [HttpGet]
        public ActionResult test(string digits)
        {
            var response = new TwilioResponse();

            response.Say(string.Format("Than you! Your choice was {0}. Have a nice day.", digits), myVoice)
                .Pause(2)
                .Hangup();

            return TwiML(response);
        }
    }
}