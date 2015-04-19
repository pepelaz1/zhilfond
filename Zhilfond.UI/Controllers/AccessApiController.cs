using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;

namespace UI.Controllers
{
    public class AccessApiController : ApiController
    {
        static readonly IUsersRepository _users_repository = new UsersRepository();
        static readonly IAccessRepository _repository = new AccessRepository();

        // GET api/accessapi
        public dynamic Get()
        {
            try
            {

                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).First();

                UserV u = _users_repository.GetVByToken(token);

                var col = (from x in Request.GetQueryNameValuePairs()
                           where x.Key == "id_house"
                           select x.Value);

                if (col.Count() > 0)
                {
                    return _repository.Get(u, int.Parse(col.First().ToString()));
                }
                else
                {



                    string form = (from x in Request.GetQueryNameValuePairs()
                                   where x.Key == "form"
                                   select x.Value).First();

                    string category = (from x in Request.GetQueryNameValuePairs()
                                       where x.Key == "category"
                                       select x.Value).First();

                    return _repository.Get(u, form, category);               
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 
        // GET api/accessapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/accessapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/accessapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/accessapi/5
        public void Delete(int id)
        {
        }
    }
}
