using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Newtonsoft.Json.Linq;

namespace UI.Controllers
{
    public class MessagesApiController : ApiController
    {
        static readonly IMessagesRepository _repository = new MessagesRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();

        // GET api/messagesapi
        public dynamic Get()
        {
            try
            {
                int id_house = int.Parse((from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "id_house"
                                         select x.Value).FirstOrDefault().ToString());

               
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();

                User user = _users_repository.GetByToken(token);

                return _repository.Get(id_house, user.Id);

            }
            catch (Exception ex)
            {
                return new List<MessageV>();
            }
        }

        // GET api/messagesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/messagesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                string token = json.Token;
                int id_house = json.Id_house;
                string text = json.Text;

                User user = _users_repository.GetByToken(token);
                Message m = new Message()
                {
                    Id_user = user.Id,
                    Id_house = id_house,
                    Text = text,
                    Created = DateTime.Now
                };
                _repository.Add(m);
       
                var response = Request.CreateResponse<ZValue>(HttpStatusCode.OK, null);
                string uri = Url.Link("DefaultApi", new { id = -1 });
                response.Headers.Location = new Uri(uri);

                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/messagesapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/messagesapi/5
        public void Delete(int id)
        {
        }
    }
}
