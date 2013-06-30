using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using RestSharp;
using PreyCore;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PreyClient.Utilities
{
    public class PreyApi
    {
        public void CreateAccount(string name, string email, string password, string passwordConfirm, string refererUserId = "")
        {
            PostMultiPartFormData postRequest = new PostMultiPartFormData();

            postRequest.PostComplete += new EventHandler( (sender, e) => 
            {
                IAsyncResult ar = ((IAsyncResult)sender);

                using (WebResponse resp = ((HttpWebRequest)ar.AsyncState).EndGetResponse(ar))
                {

                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {

                        string responseString = sr.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine(responseString);
                        

                    }
                }
            });

            postRequest.Post("https://user.share.cl/users.xml", 
                new Dictionary<string, object>() {
                    { "name", name } ,
                    { "email", email } ,
                    { "password", password } ,
                    { "password_confirmation", passwordConfirm } ,
                    { "referer_user_id", refererUserId } ,

                });
        }

        public void GetAccount(string username, string password)
        {
            WebClient wc = new WebClient();
            wc.Credentials = new NetworkCredential(username, password);
            wc.DownloadStringCompleted += (o, e) => {
                if (e.Error == null)
                {
                    XDocument xdoc = XDocument.Parse(e.Result, LoadOptions.None);

                    System.Diagnostics.Debug.WriteLine(xdoc.FirstNode.ToString());
                }
            };
            wc.DownloadStringAsync(new Uri("http://panel.preyproject.com/profile.xml"));
            byte[] myDeviceID = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
            string idAsString = Convert.ToBase64String(myDeviceID);
            System.Diagnostics.Debug.WriteLine(idAsString);


//            private void DoWebRequestWithCredentials()
//{
//    WebRequest.RegisterPrefix("http://", System.Net.Browser.WebRequestCreator.ClientHttp);
//    var request = WebRequest.Create(new Uri("http://mark.mymonster.nl")) as HttpWebRequest;
//    request.Credentials = new NetworkCredential("username", "password");
//    request.UseDefaultCredentials = false;
//    request.BeginGetResponse(ResponseCallBack, request);
//}
 
//private void ResponseCallBack(IAsyncResult ar)
//{
//    var request = ar.AsyncState as HttpWebRequest;
//    var response = request.EndGetResponse(ar) as HttpWebResponse;
//    using(var reader = new StreamReader(response.GetResponseStream()))
//    {
//        string result = reader.ReadToEnd();
//    }
//}
        }
        
        
    }
}
