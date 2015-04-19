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
    public class DictsApiController : ApiController
    {
        static readonly IDictsRepository _repository = new DictsRepository();

        // GET api/dictsapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                //String searchField = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchField").Value;
                //String searchOper = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchOper").Value;
                //String searchString = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchString").Value;

                var dicts = _repository.GetAll(/*sidx, sord, searchField, searchOper, searchString*/);

               // var pageIndex = 1; // Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = dicts.Count();
                var totalPages = 1; // (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                //dicts = dicts.Skip(pageIndex * pageSize).Take(pageSize);
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from dict in dicts
                        select new
                        {
                            i = dict.Id.ToString(),
                            cell = new string[] 
                            {
                               dict.Id.ToString(),
                               //key.Order.ToString(),
                               dict.Title,
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

        // GET api/dictsapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dictsapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;
                Dict newdict = jsonData.ToObject<Dict>();
                _repository.Add(newdict);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/dictsapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                Dict dict = jsonData.ToObject<Dict>();
                _repository.Update(id, dict);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/dictsapi/5
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
