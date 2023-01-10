using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using System.Reflection;

namespace MEDbot.Controllers
{


    public class MedicalChatController : Controller


    {

        // GET: MedicalChat
        public ActionResult Index()
        {
            Session.Clear();
            Random rnd = new Random();


            var Verification = rnd.Next(1000, 9999);

            Session["Verification"] = Verification;


            return View();
        }
            

        int sessionCount;

        [HttpPost]
        public async Task<ActionResult> Index(string txt)
        {


           var VerificationRobot = Session["Verification"].ToString();

            try
            {


                Data data = new Data();
                string firstUrl = "https://api.infermedica.com/v3/parse";
                string secondUrl = "https://api.infermedica.com/v3/diagnosis";
                var appID = "####";
                var appKey = "####";


                if (txt == "exit")
                {


                    Session["robot"] = null;
                    Session["gotName"] = null;
                    Session["gotAge"] = null;
                    Session["gotSex"] = null;
                    Session["gotDesc"] = null;
                    Session["name"] = null;
                    Session["age"] = null;
                    Session["sex"] = null;
                    Session["desc"] = null;
                    Session["gotEvidence_1"] = null;
                    Session["Evidence_1"] = null;
                    Session["gotEvidence_2"] = null;
                    Session["Evidence_2"] = null;
                    Session["gotEvidence_3"] = null;
                    Session["Evidence_3"] = null;
                    Session["gotEvidence_4"] = null;
                    Session["Evidence_4"] = null;
                    Session["gotEvidence_5"] = null;
                    Session["Evidence_5"] = null;
                    Session["gotEvidence_6"] = null;
                    Session["Evidence_6"] = null;
                    Session["gotEvidence_7"] = null;
                    Session["Evidence_7"] = null;
                    Session["gotEvidence_8"] = null;
                    Session["Evidence_8"] = null;
                    Session["gotEvidence_9"] = null;
                    Session["Evidence_9"] = null;
                    Session["gotEvidence_10"] = null;
                    Session["Evidence_10"] = null;
                    Session["LastResponse"] = null;


                    Random rnd = new Random();
                    var Verification = rnd.Next(1000, 9999);

                    Session["Verification"] = Verification;


                    return Json(new { success = false });
                }

          
                if (Session["robot"] == null)
                {
                    Session["robot"] = false;
                    Session["gotName"] = false;
                    Session["gotAge"] = false;
                    Session["gotSex"] = false;
                    Session["gotDesc"] = false;
                    Session["gotEvidence_1"] = false;
                    Session["gotEvidence_2"] = false;
                    Session["gotEvidence_3"] = false;
                    Session["gotEvidence_4"] = false;
                    Session["gotEvidence_5"] = false;
                    Session["gotEvidence_6"] = false;
                    Session["gotEvidence_7"] = false;
                    Session["gotEvidence_8"] = false;
                    Session["gotEvidence_9"] = false;
                    Session["gotEvidence_10"] = false;

                    return Json(new { success = true, msg = "are you robot ", msg2 = "if you not robot write this number!", msg3 = VerificationRobot, flagExit = false }) ;

                }
             




                if ((bool)Session["robot"] == false )
                {

                    counter++;

                
                    int validRoot = 0;
                    bool digit = int.TryParse(txt, out validRoot);
                    if (digit == false)
                    {
                        Random rnd = new Random();
                        var Verification = rnd.Next(1000, 9999);
                        Session["Verification"] = Verification;
                        VerificationRobot = Session["Verification"].ToString();
                        return Json(new { success = true, msg = "Please enter the number valid", msg2 = VerificationRobot });

                    }

                    if (counter >= 3 && txt != VerificationRobot)
                    {
                        return Json(new { success = false, msg = "You have exhausted your verification attempts", msg2 = "send exit to again"  });                       
                    }

                   
                    if ( txt != VerificationRobot)
                    {
                       Random rnd = new Random();
                        var Verification = rnd.Next(1000, 9999);
                        Session["Verification"] = Verification;
                        VerificationRobot = Session["Verification"].ToString();
                        return Json(new { success = true, msg = " Invalid code, try again", msg2 = VerificationRobot });
                    }                                   

                    Session["robot"] = true;

                    return Json(new { success = true, msg = "Welcome to the health guidance bot ", msg2 = "Warning This bot is intended for medical guidance only, and is not taken as medical advice", msg3 = "What is your name?" });

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

                    return Json(new { success = true, msg = "Are you male or female?" });

                }

                if ((bool)Session["gotSex"] == false)
                {
                    string strRegex = @"(male)|(female)|(f)|(m)|(w)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Please enter valid gender ( male / m or female / f )" });
                    }
                    if (txt.StartsWith("m"))
                    {
                        txt = "male";
                    }
                    if(txt.StartsWith("f") || txt.StartsWith("w"))
                    {
                        txt = "female";
                    }

                    Session["gotSex"] = true;
                    Session["sex"] = txt;

                    return Json(new { success = true, msg = "Please explain your issue" });

                }

                if ((bool)Session["gotDesc"] == false)
                {

                    Session["gotSeaceh"] = true;
                    Session["desc"] = txt;
                }

                if ((bool)Session["gotEvidence_1"] == false)
                {

                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        age = new { value = Session["age"] },
                        text = Session["desc"]
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                  

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(firstUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    if (responseText.Contains("message"))
                    {
                        return Json(new { success = true, msg = "error, please try again later" });
                    }
                    if (!responseText.Contains("name"))
                    {
                        return Json(new { success = true, msg = "please explain better to make us help you" });
                    }

                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(responseText);

                    var evid = new Evidence();
                    evid.id = data.mentions[0].id;
                    evid.choice_id = data.mentions[0].choice_id;
                    Session["Evidence_1"] = evid;

                    Session["gotEvidence_1"] = true;

                }


                if ((bool)Session["gotEvidence_2"] == false)
                {
                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id}
                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = data.mentions[0].choice_id;
                    Session["Evidence_2"] = evid;
                    Session["gotEvidence_2"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });

                }

                if ((bool)Session["gotEvidence_3"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id}
                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_3"] = evid;
                    Session["gotEvidence_3"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });

                }


                //qustions 2
                if ((bool)Session["gotEvidence_4"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id},
                            new {id=((Evidence)Session["Evidence_3"]).id, choice_id=((Evidence)Session["Evidence_3"]).choice_id}

                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_4"] = evid;
                    Session["gotEvidence_4"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });






                }



                //qustions 3
                if ((bool)Session["gotEvidence_5"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id},
                            new {id=((Evidence)Session["Evidence_3"]).id, choice_id=((Evidence)Session["Evidence_3"]).choice_id},
                             new {id=((Evidence)Session["Evidence_4"]).id, choice_id=((Evidence)Session["Evidence_4"]).choice_id}


                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    //var data2= Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);
                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_5"] = evid;
                    Session["gotEvidence_5"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });


                }

                //qes

                if ((bool)Session["gotEvidence_6"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id},
                            new {id=((Evidence)Session["Evidence_3"]).id, choice_id=((Evidence)Session["Evidence_3"]).choice_id},
                             new {id=((Evidence)Session["Evidence_4"]).id, choice_id=((Evidence)Session["Evidence_4"]).choice_id}
                             , new {id=((Evidence)Session["Evidence_5"]).id, choice_id=((Evidence)Session["Evidence_5"]).choice_id}


                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    //var data2= Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);
                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_6"] = evid;
                    Session["gotEvidence_6"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });






                }



                if ((bool)Session["gotEvidence_7"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id},
                            new {id=((Evidence)Session["Evidence_3"]).id, choice_id=((Evidence)Session["Evidence_3"]).choice_id},
                             new {id=((Evidence)Session["Evidence_4"]).id, choice_id=((Evidence)Session["Evidence_4"]).choice_id}
                             , new {id=((Evidence)Session["Evidence_5"]).id, choice_id=((Evidence)Session["Evidence_5"]).choice_id}
                        ,new {id=((Evidence)Session["Evidence_6"]).id, choice_id=((Evidence)Session["Evidence_6"]).choice_id}


                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    //var data2= Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);
                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_7"] = evid;
                    Session["gotEvidence_7"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });






                }
                if ((bool)Session["gotEvidence_8"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id},
                            new {id=((Evidence)Session["Evidence_3"]).id, choice_id=((Evidence)Session["Evidence_3"]).choice_id},
                             new {id=((Evidence)Session["Evidence_4"]).id, choice_id=((Evidence)Session["Evidence_4"]).choice_id}
                             , new {id=((Evidence)Session["Evidence_5"]).id, choice_id=((Evidence)Session["Evidence_5"]).choice_id}
                        ,new {id=((Evidence)Session["Evidence_6"]).id, choice_id=((Evidence)Session["Evidence_6"]).choice_id}
                       ,new {id=((Evidence)Session["Evidence_7"]).id, choice_id=((Evidence)Session["Evidence_7"]).choice_id}


                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    //var data2= Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);
                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_8"] = evid;
                    Session["gotEvidence_8"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });






                }



                if ((bool)Session["gotEvidence_9"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id},
                            new {id=((Evidence)Session["Evidence_3"]).id, choice_id=((Evidence)Session["Evidence_3"]).choice_id},
                             new {id=((Evidence)Session["Evidence_4"]).id, choice_id=((Evidence)Session["Evidence_4"]).choice_id}
                             , new {id=((Evidence)Session["Evidence_5"]).id, choice_id=((Evidence)Session["Evidence_5"]).choice_id}
                        ,new {id=((Evidence)Session["Evidence_6"]).id, choice_id=((Evidence)Session["Evidence_6"]).choice_id}
                       ,new {id=((Evidence)Session["Evidence_7"]).id, choice_id=((Evidence)Session["Evidence_7"]).choice_id}
                 ,new {id=((Evidence)Session["Evidence_8"]).id, choice_id=((Evidence)Session["Evidence_8"]).choice_id}


                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    //var data2= Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);
                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_9"] = evid;
                    Session["gotEvidence_9"] = true;

                    return Json(new { success = true, msg = q, msg2 = " Enter ( 1=yes, 2=no, 3=don't know )" });






                }




                if ((bool)Session["gotEvidence_10"] == false)
                {
                    string strRegex = @"(1)|(2)|(3)";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txt))
                    {
                        return Json(new { success = true, msg = "Valid Answers ( 1=yes, 2=no, 3=don't know )" });
                    }
                    string choice_id = "";

                    switch (txt)
                    {
                        case "1":
                            {
                                choice_id = "present";
                                break;
                            }
                        case "2":
                            {
                                choice_id = "absent";
                                break;
                            }
                        case "3":
                            {
                                choice_id = "unknown";
                                break;
                            }
                    }


                    var rmsJson = new JavaScriptSerializer().Serialize(new
                    {
                        sex = Session["sex"],
                        age = new { value = Session["age"] },
                        evidence = new[]
                        {
                            new {id=((Evidence)Session["Evidence_1"]).id, choice_id=((Evidence)Session["Evidence_1"]).choice_id},
                            new {id=((Evidence)Session["Evidence_2"]).id, choice_id=((Evidence)Session["Evidence_2"]).choice_id},
                            new {id=((Evidence)Session["Evidence_3"]).id, choice_id=((Evidence)Session["Evidence_3"]).choice_id},
                             new {id=((Evidence)Session["Evidence_4"]).id, choice_id=((Evidence)Session["Evidence_4"]).choice_id}
                             , new {id=((Evidence)Session["Evidence_5"]).id, choice_id=((Evidence)Session["Evidence_5"]).choice_id}
                        ,new {id=((Evidence)Session["Evidence_6"]).id, choice_id=((Evidence)Session["Evidence_6"]).choice_id}
                       ,new {id=((Evidence)Session["Evidence_7"]).id, choice_id=((Evidence)Session["Evidence_7"]).choice_id}
                 ,new {id=((Evidence)Session["Evidence_8"]).id, choice_id=((Evidence)Session["Evidence_8"]).choice_id}
                                  ,new {id=((Evidence)Session["Evidence_9"]).id, choice_id=((Evidence)Session["Evidence_9"]).choice_id}


                        },
                        extras = new
                        {
                            disable_groups = true,
                            interview_mode = "triage"
                        }
                    });

                    byte[] postBytes = Encoding.ASCII.GetBytes(rmsJson);

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(secondUrl);
                    request.Headers.Add("App-Id", appID);
                    request.Headers.Add("App-Key", appKey);
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
                    Session["LastResponse"] = responseText;

                    //var data2= Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);
                    var d = JObject.Parse(responseText);
                    var evid = new Evidence();
                    string q = (string)d.SelectToken("question.text");
                    string condition = (string)d.SelectToken("conditions[0].name");
                    evid.id = (string)d.SelectToken("question.items[0].id");
                    evid.choice_id = choice_id;
                    Session["Evidence_10"] = evid;
                    Session["gotEvidence_10"] = true;
                    if (condition != null)
                    {
                        return Json(new { success = true, msg = "Conversation is over", msg2 = "Based on your previous answer, it turns out that you have: " + condition, msg3 = "it was my pleasure assisting you , We hope you are always in good health ,If you want anything else just write \"exit\" and we will begin again or Have a good day Sir" });
                    }
                    else
                    {
                        return Json(new { success = true, msg = "Sorry, It seems error occured, please try again later" });

                    }
                    if (txt != null)
                    {
                        return Json(new { msg = "Diagnosis: " + data.mentions[0].name });

                    }

                }

                //   سوي شرط اذا الناتج  null يكتب نعتذر
                return Json(new { success = true, msg = "Diagnosis: " + data.mentions[0].name });

            }


            catch (Exception ex)
            {

                var error = ex.ToString();
                return Json(new { success = false, msg = "Sorry, It seems error occured, please try again later", msg2 = "Enter 'exit' To restart the bot" });
            }

            //next qus






        }



        #region first response
        private class Data
        {
            public Mentions[] mentions { get; set; }
            public bool obvious { get; set; }
        }

        private class Mentions
        {


            public string id { get; set; }
            public string name { get; set; }
            public string common_name { get; set; }
            public string choice_id { get; set; }

        }
        #endregion

        private class Evidence
        {
            public string id { get; set; }
            public string choice_id { get; set; }
        }


 

        private class Question
        {
            public string type { get; set; }
            public string text { get; set; }
            public Items[] items { get; set; }

        }

        private class Items
        {
            public string id { get; set; }
            public string name { get; set; }
            public Choices[] choices { get; set; }

        }

        private class Choices
        {
            public string id { get; set; }
            public string label { get; set; }

        }

        private class Conditions
        {
            public string id { get; set; }
            public string name { get; set; }
            public string common_name { get; set; }
            public double probability { get; set; }

        }
        //private bool flagExit = false;
        
        
      
        
        
        private int counter
        {

            get
            {
                return Session["Count"] == null ? 0 : (int)Session["Count"];
            }
            set
            {
                Session["Count"] = value;
            }

        }
    }
}