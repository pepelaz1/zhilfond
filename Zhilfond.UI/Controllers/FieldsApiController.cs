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
    public class FieldsApiController : ApiController
    {
        static readonly IFieldsRepository _repository = new FieldsRepository();

        // GET api/fieldsapi
        public dynamic Get()
        {
            try
            {
               string id_form = (from x in Request.GetQueryNameValuePairs()
                                 where x.Key == "id_form"
                                 select x.Value).FirstOrDefault();

                IEnumerable<string> col  = from x in Request.GetQueryNameValuePairs()
                                           where x.Key == "type"
                                           select x.Value;
                if (col.Count() > 0 )
                {
                    string type = col.FirstOrDefault();
                    if (type == "details")
                    {
                        return _repository.GetDetails(int.Parse(id_form));
                    }
                }           

                var fieldsv = _repository.Get(int.Parse(id_form));

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
                               fld.Title,
                               fld.Order == null ? "" : fld.Order.ToString(),
                               fld.Typename,
                               fld.Category,
                               fld.Required.ToString(),
                               fld.Personal.ToString()
                               }
                        }).ToArray()
                };
            }
            catch (Exception )
            {
                return null;
            }
        }

       

        // GET api/fieldsapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/fieldsapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                dynamic o = jsonData;
                o.id = -1;
                o.Required = (o.Required == "on" || o.Required == "Yes") ? "true" : "false";
                o.Personal = (o.Personal == "on" || o.Personal == "Yes") ? "true" : "false";

                ZField field = o.ToObject<ZField>();
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_form")
                        field.Id_form = int.Parse(pair.Value);
                }
                string id_category_s = o.Id_category;
                _repository.Add(field, int.Parse(id_category_s));
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/fieldsapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                dynamic o = jsonData;
                o.Required = (o.Required == "on" || o.Required == "Yes") ? "true" : "false";
                o.Personal = (o.Personal == "on" || o.Personal == "Yes") ? "true" : "false";

                ZField field = o.ToObject<ZField>();
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_form")
                        field.Id_form = int.Parse(pair.Value);
                }
            
                string id_category_s = o.Id_category;
                _repository.Update(id, field, int.Parse(id_category_s));
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
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
