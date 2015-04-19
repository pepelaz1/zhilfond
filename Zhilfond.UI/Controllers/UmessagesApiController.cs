using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UI.Controllers
{
    public class UmessagesApiController : ApiController
    {
        static readonly IUmessagesRepository _repository = new UmessagesRepository();
        static readonly IUsersRepository _rep_users = new UsersRepository();             

        // GET api/umessagesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                var umessagesv = _repository.Get(Request.GetQueryNameValuePairs());

                var pageIndex = Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = umessagesv.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                umessagesv = umessagesv.Skip(pageIndex * pageSize).Take(pageSize);

                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from um in umessagesv
                        select new
                        {
                            i = um.Id.ToString(),
                            cell = new string[] 
                            {
                               um.Id.ToString(),
                               um.Id_house.ToString(),
                               um.Login,
                               um.WhenDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                               um.Text
                             }
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic Get()
        {
            try
            {
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();

                User user = _rep_users.GetByToken(token);
                var cnt = _repository.GetCount(user.Id);
                return new
                {
                    Count = cnt               
                };
            }
            catch (Exception)
            {
                return null;
            }
        }



        // POST api/dictsvaluesapi/5
       /* public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;                

                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_dict")
                        ((dynamic)jsonData).Id_dict = int.Parse(pair.Value);
                }

                DictValue newdictvalue = jsonData.ToObject<DictValue>();

                newdictvalue = _repository.Add(newdictvalue);
                return newdictvalue.Id != -1 ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.Conflict);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/dictsvaluesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_dict")
                        ((dynamic)jsonData).Id_dict = int.Parse(pair.Value);
                }

                DictValue dictvalue = jsonData.ToObject<DictValue>();

                return _repository.Update(id, dictvalue) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.Conflict);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }*/

        // DELETE api/umessagesapi/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _repository.Delete(id);

                var response = Request.CreateResponse<UnreadMessage>(HttpStatusCode.OK, null);
                string uri = Url.Link("DefaultApi", new { id = -1 });
                response.Headers.Location = new Uri(uri);

                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
