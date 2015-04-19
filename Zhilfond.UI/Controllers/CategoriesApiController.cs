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
    public class CategoriesApiController : ApiController
    {
        static readonly ICategoriesRepository _repository = new CategoriesRepository();

        // GET api/categoriesapi
        public dynamic Get()
        {
            try
            {
 
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

                var fieldsv = _repository.GetAll();

                int page = 1;
                var pageIndex = Convert.ToInt32(page) - 1;
                var totalRecords = fieldsv.Count();
                var totalPages = 1;
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from fld in fieldsv
                        select new
                        {
                            i = fld.Id.ToString(),
                            cell = new string[] 
                            {
                               fld.Id.ToString(),
                               fld.Order.ToString(),
                               fld.Title,                              
                             }
                        }).ToArray()
                };
            }
            catch (Exception)
            {
                return new List<Category>();
            }
        }


        // GET api/categoriesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/categoriesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;
                Category cat = jsonData.ToObject<Category>();
                _repository.Add(cat);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/categoriesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                Category cat = jsonData.ToObject<Category>();
                _repository.Update(id, cat);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/categoriesapi/5
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
