using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Models;
using UI.Classes;
using UI.Filters;
using DAL.Interfaces;
using DAL.Repositories;
using Newtonsoft.Json.Linq;

namespace UI.Controllers
{
    public class RolesApiController : ApiController
    {
        static readonly IRolesRepository _repository = new RolesRepository();

        // GET api/rolesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                //String searchField = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchField").Value;
                //String searchOper = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchOper").Value;
                //String searchString = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchString").Value;

                IEnumerable<string> col = from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "type"
                                          select x.Value;
                if (col.Count() > 0)
                {
                    string type = col.FirstOrDefault();
                    if (type == "plain")
                    {
                        return _repository.GetAll();
                    }
                }

                var roles = _repository.GetAll(/*sidx, sord, searchField, searchOper, searchString*/);

                // var pageIndex = 1; // Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = roles.Count();
                var totalPages = 1; // (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                //roles = roles.Skip(pageIndex * pageSize).Take(pageSize);
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from role in roles
                        select new
                        {
                            i = role.Id.ToString(),
                            cell = new string[] 
                            {
                               role.Id.ToString(),
                               //rolevalue.Order.ToString(),
                               role.Title,
                            }
                        }).ToArray()
                };
            }
            catch (Exception)
            {
                return new List<Role>();
            }
        }

        public dynamic Get()
        {
            try
            {
                return _repository.GetAllDesc();
            }
            catch (Exception)
            {
                return new List<Role>();
            }
        }

        // GET api/rolesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/rolesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;
                Role newrole = jsonData.ToObject<Role>();
                _repository.Add(newrole);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/rolesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                Role role = jsonData.ToObject<Role>();
                _repository.Update(id, role);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/rolesapi/5
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
