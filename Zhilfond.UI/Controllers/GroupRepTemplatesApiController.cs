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
    public class GroupRepTemplatesApiController : ApiController
    {
        static readonly IGroupRepTemplatesRepository _repository = new GroupRepTemplatesRepository();

        // GET api/groupreptemplatesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                var groupreptemplates = _repository.GetAll(/*sidx, sord, searchField, searchOper, searchString*/);
                var pageSize = rows;
                var totalRecords = groupreptemplates.Count();
                var totalPages = 1;
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from groupreptemplate in groupreptemplates
                        select new
                        {
                            i = groupreptemplate.Id.ToString(),
                            cell = new string[] 
                            {
                               groupreptemplate.Id.ToString(),
                               groupreptemplate.template_name,
                            }
                        }).ToArray()
                };
            }
            catch (Exception)
            {
                return new List<GroupRepTemplate>();
            }
        }

        public dynamic Get()
        {
            try
            {
                return _repository.GetAll();
            }
            catch (Exception)
            {
                return new List<Dict>();
            }
        }

        // GET api/groupreptemplatesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/groupreptemplatesapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;
                GroupRepTemplate newTempl = jsonData.ToObject<GroupRepTemplate>();
                _repository.Add(newTempl);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/groupreptemplatesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                GroupRepTemplate templ = jsonData.ToObject<GroupRepTemplate>();
                _repository.Update(id, templ);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/groupreptemplatesapi/5
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
