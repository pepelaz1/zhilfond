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
    public class DictsValuesApiController : ApiController
    {
        static readonly IDictsValuesRepository _repository = new DictsValuesRepository();
             

        // GET api/dictsvaluesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                var values = _repository.Get(Request.GetQueryNameValuePairs());

                var pageIndex = Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = values.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                values = values.Skip(pageIndex * pageSize).Take(pageSize);

                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from fld in values
                        select new
                        {
                            i = fld.Id.ToString(),
                            cell = new string[] 
                            {
                               fld.Id.ToString(),
                               fld.Value                               
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
                string id_dict = (from x in Request.GetQueryNameValuePairs()
                                  where x.Key == "id_dict"
                                  select x.Value).FirstOrDefault();

                IEnumerable<string> col = from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "type"
                                          select x.Value;
                var values = _repository.GetByDict(int.Parse(id_dict));


              //  var pageIndex = Convert.ToInt32(page) - 1;
             //   var pageSize = rows;
                var totalRecords = values.Count();
              //  var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
              //  values = values.Skip(pageIndex * pageSize).Take(pageSize);

                return new
                {
                    total = 1,
                    page = 1,
                    records = totalRecords,
                    rows = (
                        from fld in values
                        select new
                        {
                            i = fld.Id.ToString(),
                            cell = new string[] 
                            {
                               fld.Id.ToString(),
                               fld.Value                               
                               }
                        }).ToArray()
                };
            }
            catch (Exception)
            {
                return null;
            }
        }



        // POST api/dictsvaluesapi/5
        public HttpResponseMessage Post(JObject jsonData)
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
        }

        // DELETE api/dictsvaluesapi/5
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
