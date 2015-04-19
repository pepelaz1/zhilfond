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
    public class FormulasApiController : ApiController
    {
        static readonly IFormulasRepository _repository = new FormulasRepository();

        // GET api/formulasapi
        public dynamic Get()
        {
            try
            {
                string id_field = (from x in Request.GetQueryNameValuePairs()
                                  where x.Key == "id_field"
                                  select x.Value).FirstOrDefault();

                var formulas = _repository.Get(int.Parse(id_field));

                int page = 1;
                var pageIndex = Convert.ToInt32(page) - 1;
                var totalRecords = formulas.Count();
                var totalPages = 1;
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from f in formulas
                        select new
                        {
                            i = f.Id.ToString(),
                            cell = new string[] 
                            {
                               f.Id.ToString(),
                               f.Title,
                               f.Predicate
                            }
                        }).ToArray()
                };
            }
            catch (Exception )
            {
                return new List<ZFieldV>();
            }
        }

        // GET api/formulasapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/formulasapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                dynamic o = jsonData;
                o.id = -1;

                Formula f = o.ToObject<Formula>();
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_field")
                        f.Id_field = int.Parse(pair.Value);
                }
                _repository.Add(f);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/formulasapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                dynamic o = jsonData;

                Formula f = o.ToObject<Formula>();
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_field")
                        f.Id_field = int.Parse(pair.Value);
                }

                _repository.Update(id, f);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/formulasapi/5
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
