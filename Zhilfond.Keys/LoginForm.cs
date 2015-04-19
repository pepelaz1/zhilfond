using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json;
using System.IO;
using System.Web.Script.Serialization;

namespace Zhilfond.Keys
{
    public partial class LoginForm : System.Windows.Forms.Form
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Token { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private bool Authorize(string username, string password)
        {
            Cursor = Cursors.WaitCursor;
            try
            {

                string serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
                string tokenApi = "/api/tokensapi/";

                dynamic o = new ExpandoObject();
                o.Username = username;
                o.Password = password;
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
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var data = reader.ReadToEnd();
                        dynamic res = js.DeserializeObject(data);
                        Token = res["Key"];
                    }
                    response.Close();
                }
                return true;
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Authorize(textBox1.Text, textBox2.Text))
            {
                Username = textBox1.Text;
                Password = textBox2.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                MessageBox.Show("Ошибка авторизации");
        }
    }
}
