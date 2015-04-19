using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using ClosedXML.Excel;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using UI.Classes;
using System.Reflection;
using System.ComponentModel;

namespace UI.Controllers
{
    public class DetailReportsApiController : ApiController
    {
        static readonly IUsersRepository _users_repository = new UsersRepository();
        static readonly IGenReportsRepository _genrep_repository = new GenReportsRepository();

        // GET api/detailreportsapi
        public HttpResponseMessage Get()
        {
            try
            {
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();

                var u = _users_repository.GetByToken(token);

                int tmplId = int.Parse((from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "tmplId"
                                          select x.Value).FirstOrDefault().ToString());

                int id_elem = int.Parse((from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "elemId"
                                          select x.Value).FirstOrDefault().ToString());

                int nReportType = int.Parse((from x in Request.GetQueryNameValuePairs()
                                             where x.Key == "type"
                                             select x.Value).FirstOrDefault().ToString());

                string file_name = (from x in Request.GetQueryNameValuePairs()
                                    where x.Key == "fileName"
                                    select x.Value).FirstOrDefault().ToString();

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var detailreportbuilder = new DetailReportBuilder();
                var memoryStream = new MemoryStream();

                detailreportbuilder.Generate(id_elem, tmplId, nReportType, memoryStream);
                
                string filename = (string.IsNullOrEmpty(file_name)) ? "report.xlsx" : file_name;

                ////////////// Сохраняем в базу генериный отчет ///////////////////
                var gr = new GenReport()
                {
                    Id_user = u.Id,
                    Filename = filename,
                    Created = DateTime.Now,
                    Type = "report"
                };

                gr.Data = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                gr.Data = Utils.StreamToArray(memoryStream);
                _genrep_repository.Add(gr);
                ///////////////////////////////////////////////////

                memoryStream.Position = 0;
                result.Content = new StreamContent(memoryStream);

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    //FileName = HttpUtility.UrlEncode(filename)
                    FileName = filename
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        // GET api/detailreportsapi/5
        public string Get(int id)
        {
            return "default value";
        }

        // POST api/detailreportsapi
        public void Post([FromBody]string value)
        {            
        }

        // PUT api/detailreportsapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/detailreportsapi/5
        public void Delete(int id)
        {
        }
    }
}
