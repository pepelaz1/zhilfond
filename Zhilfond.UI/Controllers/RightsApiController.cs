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
    public class RightsApiController : ApiController
    {
        static readonly IRightsRepository _repository = new RightsRepository();

        // GET api/rightsapi
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

                var rights = _repository.GetAll(/*sidx, sord, searchField, searchOper, searchString*/);

               // var pageIndex = 1; // Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = rights.Count();
                var totalPages = 1; // (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                //rights = rights.Skip(pageIndex * pageSize).Take(pageSize);
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from right in rights
                        select new
                        {
                            i = right.Id.ToString(),
                            cell = new string[] 
                            {
                               right.Id.ToString(),                               
                               right.Title,
                            }
                        }).ToArray()
                };
            }
            catch (Exception)
            {
                return new List<Dict>();
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
                return new List<Dict>();
            }
        }       

        // GET api/rightsapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/rightsapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                //((dynamic)jsonData).id = -1;
                //Dict newdict = jsonData.ToObject<Dict>();
                //_repository.Add(newdict);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/rightsapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                //Dict dict = jsonData.ToObject<Dict>();
                //_repository.Update(id, dict);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/rightsapi/5
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
