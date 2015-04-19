using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json;
using System.IO;

namespace Zhilfond.Keys
{
    class KeyUploader
    {
        string _username;
        string _password;
        string _token;

        public KeyUploader(string token, string username, string password)
        {
            _token = token;                 
            _username = username;
            _password = password;
        }

        public void Process(string pub_cert, string priv_key)
        {
            string serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            string tokenApi = "/api/keysapi/";

            dynamic o = new ExpandoObject();
            o.Token = _token;
            o.PublicKey = pub_cert;
            o.PrivateKey = _username == "admin" ? priv_key : "";
            string json = JsonConvert.SerializeObject(o);

            var body = Encoding.UTF8.GetBytes(json);
            var request = (HttpWebRequest)WebRequest.Create(serverUrl + tokenApi);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = body.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
                stream.Close();
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                response.Close();
            }
        }
    }
}
