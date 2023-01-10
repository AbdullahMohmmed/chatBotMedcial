using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MEDbot.Controllers
{
    public class Diagnosis : Controller
    {
        public class extras
        {
            public bool disable_groups { get; set; }
            public string interview_mode { get; set; }
            
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<ActionResult>Index(string txt)

        {
            try
            {
                if (txt == "exit")
                {
                    Session["gotName"] = null;
                    Session["gotAge"] = null;
                    //Session["gotDesc"] = null;
                    Session["gotSex"] = null;
                    Session["gotSearch"] = null;
                    Session["name"] = null;
                    Session["age"] = null;
                    //Session["desc"] = null;
                    Session["sex"] = null;
                    Session["search"] = null;
                    return Json(new { success = false });
                }
                //if flag the name is it has not value
                //the bot will ask him 
                // whats your name ?

                if (Session["gotName"] == null)
                {
                    Session["gotName"] = false;
                    Session["gotAge"] = false;
                    Session["gotSearch"] = false;
                    Session["gotSex"] = false;

                    return Json(new { success = true, msg = "What is your name?" });
                }
                if ((bool)Session["gotName"] == false)
                {
                    if (txt.Length < 3)
                    {
                        return Json(new { success = true, msg = "Need a real name, more than 3 letters" });
                    }
                    Session["gotName"] = true;
                    Session["name"] = txt;

                    return Json(new { success = true, msg = "How old are you " + txt + "?" });
                }

                if ((bool)Session["gotAge"] == false)
                {
                    int age = 0;
                    bool digit = int.TryParse(txt, out age);
                    if (digit == false)
                    {
                        return Json(new { success = true, msg = "Please enter valid age" });
                    }
                    if (age < 12 || age > 130)
                    {
                        return Json(new { success = true, msg = "Please enter age between 12 and 130" });
                    }

                    Session["gotAge"] = true;
                    Session["age"] = age;

                    return Json(new { success = true, msg = "You are male or female ? " });

                }

                if ((bool)Session["gotSex"] == false)
                {
                    if (txt != "male" || txt != "female")
                    {
                        return Json(new { success = true, msg = "Enter your sex male or female !!" });

                    }
                    Session["gotSex"] = true;
                    Session["sex"] = txt;
                    return Json(new { success = true, msg = "Mention two diseases" });
                }
                if ((bool)Session["gotSearch"] == false)
                {

                    Session["gotSearch"] = true;
                    Session["search"] = txt;

                }


                var _extras = new extras();
                _extras.interview_mode = "triage";
                _extras.disable_groups = true;


                var rmsJson = new JavaScriptSerializer().Serialize(new
                {
                    sex = Session["sex"],
                    age = new { value = Session["age"] },
                    evidence = Session["serch"],
                    extras = _extras

                }
            );

                byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);


                //var sehabUrl = FL.GetConfigurationValue("Sehab_Url");
                var url = "https://api.infermedica.com/v3/diagnosis";
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Headers.Add("App-Id", "fb4f3786");
                request.Headers.Add("App-Key", "9c8500ae4f8e0735d130290da94d96b2");
                request.Method = "POST";
                request.ContentLength = postBytes.Length;
                request.ContentType = "application/json; charset=utf-8";
                request.GetRequestStream().Write(postBytes, 0, postBytes.Length);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                var encoding = ASCIIEncoding.UTF8;

                string responseText = "";
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }
                if (responseText.Contains("message"))
                {
                    return Json(new { success = true, msg = "error, please try again later" });
                }
                if (!responseText.Contains("name"))
                {
                    return Json(new { success = true, msg = "please explain better to make us help you" });
                }

                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(responseText);
                //var details = JObject.Parse(responseText);
                //var main = details["mentions"][0].Children(); //details.SelectToken("mentions").Children().Values("name");
                //var sub = main[0].Value;
                return Json(new { success = true, msg = "question is : " + data.question });


            }
            catch (Exception ex)
            {

                var error = ex.ToString();
                return Json(new { success = false, msg = "Sorry, It seems error occured, please try again later" });
            }

            int sequncses = 1;
            //while ( sequncses<=5)
            for (var i = 0; i < 5; i++)
            {
                sequncses++;

            }


        }




        private class Data
        {
            //public items[] items { get; set; }

            public String question { get; set; }

            public String conditions { get; set; }
            public bool has_emergency_evidence { get; set; }
            public string interview_token { get; set; }

        }       
        private class question 
        {
            public string type { get; set; }
            public String text { get; set; }
            public items[] items { get; set; }
            //public string extras { get; set; }

        }


        
        private class items
        {


            public string id { get; set; }
            public string name { get; set; }
            public choices[] choices { get; set; }


        }

        public class conditions
        {
            public string id { get; set; }
            public string name { get; set; }
            public string common_name { get; set; }
            public string probability { get; set; }
        }

        public class choices
        {
            public string id { get; set; }
            public string label { get; set; }

        }

        //public class loopOfInterview {

        //    if(Session["lop1"]==null){
        //        Sess

        //}
        //public async Task<ActionResult> interView(string loop)
        //{
        //    if (Session["lop1"] == null)
        //    {

        //        Session["lop1"] = false;
        //        Session["lop2"] = false;
        //        Session["lop3"] = false;
        //        Session["lop4"] = false;
        //        Session["lop5"] = false;

        //    }
            
        //}
        
      
    


}
}