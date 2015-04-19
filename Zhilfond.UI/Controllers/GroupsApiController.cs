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
using UI.Classes;

namespace UI.Controllers
{
    public class GroupsApiController : ApiController
    {
        static readonly IGroupsRepository _repository = new GroupsRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();

        // GET api/groupsapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
         
                string token = (from x in Request.GetQueryNameValuePairs()
                                   where x.Key == "token"
                                   select x.Value).FirstOrDefault();
                
                User user = _users_repository.GetByToken(token);
                

                var groups = _repository.Get(user.Id);

                int n = 0;
                return new
                {
                    total = 1,
                    page = 1,
                    records = groups.Count(),
                    rows = (
                        from grp in groups
                        select new
                        {
                            i = (n++).ToString(),
                            cell = new string[] 
                            {
                                grp.Id.ToString(),
                                grp.Title
                            }
                        }).ToArray()
                };
            }
            catch (Exception )
            {
                return new List<Group>();
            }
        }

     

        // POST api/groupsapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;
                Group g = jsonData.ToObject<Group>();

                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();

                User user = _users_repository.GetByToken(token);
                g.Id_user = user.Id;

                _repository.Add(g);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/groupsapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                Group g = jsonData.ToObject<Group>();
                            

                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();

                User user = _users_repository.GetByToken(token);
                g.Id_user = user.Id;

                _repository.Update(id, g);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
              return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/groupsapi/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
