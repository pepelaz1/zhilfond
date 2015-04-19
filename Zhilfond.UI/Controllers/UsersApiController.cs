using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Models;
using DAL;
using System.Data;
using System.Web;
using System.ServiceModel.Channels;
using UI.Classes;
using DAL.Interfaces;
using DAL.Repositories;
using System.Net.Http.Headers;
using UI.Filters;
using Newtonsoft.Json.Linq;

namespace UI.Controllers
{
    public class UsersApiController : ApiController
    {
        static readonly IUsersRepository _repository = new UsersRepository();

        #region Get
        //[TokenAuthorize("administrator")]
       /* public IEnumerable<User> Get()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }*/

       /* public IEnumerable<User> Get(String token)
        {
            try
            {
                if (TokenValidator.Validate(token, new String[] { "administrator" }))
                    return _repository.GetAll();
                else
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }*/

       


        // GET api/usersapi/
        public dynamic Get()
        {
            try
            {
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();

                return _repository.GetVByToken(token);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[TokenAuthorize("administrator")]
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            String searchField = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchField").Value;
            String searchOper = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchOper").Value;
            String searchString = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchString").Value;

            var users = _repository.GetAll(sidx, sord, searchField, searchOper, searchString);

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = users.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            users = users.Skip(pageIndex * pageSize).Take(pageSize);
            return new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from user in users
                    select new
                    {
                        i = user.Id.ToString(),
                        cell = new string[] {
                         user.Id.ToString(),
                         user.Login,
                         user.Username,
                         user.Active.ToString(),
                         /*user.Id_Role.ToString(),*/
                         user.RoleName                         
                      }
                    }).ToArray()
            };
        }
        #endregion

        #region Post
        private HttpResponseMessage PostInternal(User item)
        {
            try
            {
                item = _repository.Add(item);
                var response = Request.CreateResponse<User>(HttpStatusCode.Created, item);
                string uri = Url.Link("DefaultApi", new { id = item.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [TokenAuthorize("administrator")]
        private HttpResponseMessage PostAuth(User item)
        {
            return PostInternal(item);
        }
        

        public HttpResponseMessage Post(JObject jsonData)
        {
            string token = ((dynamic)jsonData).UserToken;
            ((dynamic)jsonData).id = -1;
            ((dynamic)jsonData).Active = (((dynamic)jsonData).Active == "on" || ((dynamic)jsonData).Active == "Yes") ? "true" : "false";

            User user;
            try
            {
                user = ((dynamic)jsonData).ToObject<User>();
            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (token != "" && token != null)
            {
                if (TokenValidator.Validate(token, new String[] { "administrator" }))
                    return PostInternal(user);
            }
            else
            {
                return PostAuth(user);
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        //public HttpResponseMessage Post(JObject jsonData)
        //{
        //    dynamic json = jsonData;
        //    string token = json.UserToken;

        //    json.Active = (json.Active == "on" || json.Active == "false") ? "true" : "false";

        //    User user;
        //    try
        //    {
        //        json.id = -1;
        //        user = json.ToObject<User>();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }

        //    if (token != "" && token != null)
        //    {
        //        if (TokenValidator.Validate(token, new String[] { "administrator" }))
        //            return PostInternal(user);
        //    }
        //    else
        //    {
        //        return PostAuth(user);
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        //}
        #endregion

        #region Put
        private HttpResponseMessage PutInternal(int id, User item)
        {
            try
            {
                _repository.Update(id, item);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        //[TokenAuthorize("administrator")]
        private HttpResponseMessage PutAuth(int id, User item)
        {
            return PutInternal(id, item);
        }
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            string token = ((dynamic)jsonData).UserToken;            
            ((dynamic)jsonData).Active = (((dynamic)jsonData).Active == "on" || ((dynamic)jsonData).Active == "Yes" ) ? "true" : "false";

            User user;
            try
            {
                user = ((dynamic)jsonData).ToObject<User>();
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (token != "" && token != null)
            {
                if (TokenValidator.Validate(token, new String[] { "administrator" }))
                    return PutInternal(id, user);
            }
            else
            {
                return PutAuth(id, user);
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
        #endregion

        #region Delete
        private HttpResponseMessage DeleteInternal(int id)
        {
            try
            {
                _repository.Delete(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        //[TokenAuthorize("administrator")]
        private HttpResponseMessage DeleteAuth(int id)
        {
            return DeleteInternal(id);
        }

        public HttpResponseMessage Delete(int id, JObject jsonData)
        {
            string token = ((dynamic)jsonData).UserToken;            

            if (token != "" && token != null)
            {
                if (TokenValidator.Validate(token, new String[] { "administrator" }))
                    return DeleteInternal(id);
            }
            else
            {
                return DeleteAuth(id);
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
        #endregion
    }
}
