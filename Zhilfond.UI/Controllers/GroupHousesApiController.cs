using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
    public class GroupHousesApiController : ApiController
    {
        static readonly IGroupHousesRepository _repository = new GroupHousesRepository();

        // GET api/grouphousesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                string id_group = (from x in Request.GetQueryNameValuePairs()
                                   where x.Key == "id_group"
                                   select x.Value).FirstOrDefault();

                var grouphouses = _repository.Get(int.Parse(id_group));

                int n = 0;
                return new
                {
                    total = 1,
                    page = 1,
                    records = grouphouses.Count(),
                    rows = (
                        from gh in grouphouses
                        select new
                        {
                            i = (n++).ToString(),
                            cell = new string[] 
                            {
                                gh.Id.ToString(),
                                gh.Id_house.ToString(),
                                gh.Street + " " + gh.Number.ToString() + (gh.Letter == null ? "" : "/"+gh.Letter),
                                gh.Latitude.ToString(),
                                gh.Longitude.ToString(),
                                gh.Baloon
                            }
                        }).ToArray()
                };
            }
            catch (Exception )
            {
                return new List<Group>();
            }
        }



        // GET api/grouphousesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/grouphousesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;
                GroupHouse gh = jsonData.ToObject<GroupHouse>();
                _repository.Add(gh);

                var response = Request.CreateResponse<GroupHouse>(HttpStatusCode.OK, gh);
                string uri = Url.Link("DefaultApi", new { id = gh.Id });
                response.Headers.Location = new Uri(uri);
                return response;

            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/grouphousesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/fieldsapi/5
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
