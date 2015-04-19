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
    public class RulesApiController : ApiController
    {
        static readonly IRulesRepository _repository = new RulesRepository();

        // GET api/rulesapi
        public dynamic Get()
        {
            try
            {
                string id_field = (from x in Request.GetQueryNameValuePairs()
                                  where x.Key == "id_field"
                                  select x.Value).FirstOrDefault();

                var rulesv = _repository.Get(int.Parse(id_field));

                int page = 1;
                var pageIndex = Convert.ToInt32(page) - 1;
                var totalRecords = rulesv.Count();
                var totalPages = 1;
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from rule in rulesv
                        select new
                        {
                            i = rule.Id.ToString(),
                            cell = new string[] 
                            {
                               rule.Id.ToString(),
                               rule.Title,
                               rule.Operation,
                               rule.Predicate
                               }
                        }).ToArray()
                };
            }
            catch (Exception )
            {
                return new List<ZFieldV>();
            }
        }

        // GET api/rulesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/rulesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                dynamic o = jsonData;
                o.id = -1;

                Rule rule = o.ToObject<Rule>();
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_field")
                        rule.Id_field = int.Parse(pair.Value);
                }
                _repository.Add(rule);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/rulesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                dynamic o = jsonData;

                Rule rule = o.ToObject<Rule>();
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_field")
                        rule.Id_field = int.Parse(pair.Value);
                }

                _repository.Update(id, rule);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception )
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/rulesapi/5
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
