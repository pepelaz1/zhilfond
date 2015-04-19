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
    public class ReportParamsApiController : ApiController
    {
        static readonly IReportParamsRepository _repository = new ReportParamsRepository();

        // GET api/reportparamsapi
        public dynamic Get()
        {
            try
            {

                int id_group = int.Parse((from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "id_group"
                                          select x.Value).First().ToString());

                return _repository.GetAllV(id_group);
            }
            catch (Exception )
            {
                return new List<ReportParamV>();
            }
        }


        // GET api/reportparamsapi/5
        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
              //  dynamic json = jsonData;
                ReportParam rp = jsonData.ToObject<ReportParam>();
                _repository.Put(rp);   

                var response = Request.CreateResponse<ReportParam>(HttpStatusCode.Created, rp);
                string uri = Url.Link("DefaultApi", new { id = -1 });
                response.Headers.Location = new Uri(uri);

                return response;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/reportparamsapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/reportparamsapi/5
        public void Delete(int id)
        {
        }
    }
}
