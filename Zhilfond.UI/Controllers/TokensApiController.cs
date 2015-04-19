using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Models;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using DAL.Interfaces;
using DAL.Repositories;
using System.Security.Cryptography;
using UI.Classes;

namespace UI.Controllers
{
    public class TokensApiController : ApiController
    {
        static readonly IUsersRepository _rep_users = new UsersRepository();
        static readonly ISessionsRepository _rep_sessions = new SessionsRepository();


        public HttpResponseMessage Post(Token item)
        {
            try
            {

                //Authorize
                MD5 md5 = MD5.Create();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(item.Password));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                    sb.Append(hash[i].ToString("X2"));

                String passwordHash = sb.ToString().ToLower();
                User user = _rep_users.Get(item.UserName, passwordHash);

           

                if (user == null)
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    //return new HttpResponseMessage(HttpStatusCode.Conflict);

                // Generate token
                long token = DateTime.Now.Ticks;
                _rep_sessions.Put(token.ToString(), user.Id);

                

                // Create response
                item.Key = token.ToString();
                var response = Request.CreateResponse<Token>(HttpStatusCode.Created, item);
                string uri = Url.Link("DefaultApi", new { id = item.UserName });
                response.Headers.Location = new Uri(uri);

                HttpContext.Current.Session.Add("token", token);

                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                message.Content = new StringContent(ex.Message + " 111111111111111 ");
                throw new HttpResponseException(message);
            }
        }

        public HttpResponseMessage Delete(String token)
        {
          //  HttpContext.Current.Session["token"] = null;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
