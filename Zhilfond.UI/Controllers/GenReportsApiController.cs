using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Newtonsoft.Json.Linq;
using UI.Classes;

namespace UI.Controllers
{
    public class GenReportsApiController : ApiController
    {
        static readonly IGenReportsRepository _repository = new GenReportsRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();

        // GET api/genreportsapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {         
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();
                
                User user = _users_repository.GetByToken(token);


                var genreports = _repository.GetByRoleId(user.Id_Role);

                int n = 0;
                return new
                {
                    total = 1,
                    page = 1,
                    records = genreports.Count(),
                    rows = (
                        from gr in genreports
                        select new
                        {
                            i = (n++).ToString(),
                            cell = new string[] 
                            {
                                gr.Id.ToString(),
                                gr.Filename,
                                gr.Created.ToString("dd.MM.yyyy HH:mm:ss"),
                                gr.Type,
                                gr.Signature,
                                _users_repository.Get(gr.Id_user).Username
                            }
                        }).ToArray()
                };
            }
            catch (Exception  ex)
            {
                return new List<GenReport>();
            }
        }

        // GET api/genreportsapi/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                GenReport a = _repository.GetById(id);
                MemoryStream ms = new MemoryStream();
                ms.Write(a.Data, 0, a.Data.Length);
                ms.Position = 0;

                result.Content = new StreamContent(ms);

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = a.Filename
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /*
     

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
        }*/
    }
}
