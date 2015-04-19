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
    public class RolesHousesApiController : ApiController
    {
        static readonly IRolesHousesRepository _repository = new RolesHousesRepository();

        // GET api/roleshousesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {

                int id_role = int.Parse((from x in Request.GetQueryNameValuePairs()
                                            where x.Key == "id_role"
                                            select x.Value).FirstOrDefault().ToString());

                var roleshouses = _repository.Get(id_role);
              

                int n = 0;
                return new
                {
                    total = 1,
                    page = 1,
                    records = roleshouses.Count(),
                    rows = (
                        from rh in roleshouses
                        select new
                        {
                            i = (n++).ToString(),
                            cell = new string[] 
                            {
                                rh.Id.ToString(),
                                rh.Role,
                                rh.Id_House.ToString(),
                                rh.Right
                            }
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new List<RoleHouseV>();
            }
        }

        // GET api/roleshousesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/roleshousesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;

                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_role")
                        ((dynamic)jsonData).Id_Role = int.Parse(pair.Value);
                }

                RoleHouse newvalue = jsonData.ToObject<RoleHouse>();

                newvalue = _repository.Add(newvalue);
                return newvalue.Id != -1 ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.Conflict);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/roleshousesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                //foreach (var pair in Request.GetQueryNameValuePairs())
                //{
                //    if (pair.Key == "id_role")
                //        ((dynamic)jsonData).Id_dict = int.Parse(pair.Value);
                //}

                //RoleHouseV rolehousevalue = jsonData.ToObject<RoleHouseV>();

                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_role")
                        ((dynamic)jsonData).Id_Role = int.Parse(pair.Value);
                }

                RoleHouse newvalue = jsonData.ToObject<RoleHouse>();

                return _repository.Update(id, newvalue) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.Conflict);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/roleshousesapi/5
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
