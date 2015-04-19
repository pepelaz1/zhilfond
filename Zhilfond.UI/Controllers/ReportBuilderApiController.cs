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

namespace UI.Controllers
{
    public class ReportBuilderApiController : ApiController
    {
        static readonly IUsersRepository _users_repository = new UsersRepository();
        static readonly IGenReportsRepository _genrep_repository = new GenReportsRepository();


        // GET api/reportbuilderapi
        public HttpResponseMessage Get()
        {
            try
            {
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();


                var u = _users_repository.GetByToken(token);

                int id_group = int.Parse((from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "id_group"
                                          select x.Value).FirstOrDefault().ToString());
                int id_template = int.Parse((from x in Request.GetQueryNameValuePairs()
                                             where x.Key == "id_template"
                                          select x.Value).FirstOrDefault().ToString());

                string file_name = (from x in Request.GetQueryNameValuePairs()
                                    where x.Key == "fileName"
                                    select x.Value).FirstOrDefault().ToString();


                var memoryStream = new MemoryStream();
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var rb = new ReportBuilder();
                rb.Generate(id_group, id_template, memoryStream);
                                
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
                    FileName = filename
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        // GET api/reportbuilderapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/reportbuilderapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/reportbuilderapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/reportbuilderapi/5
        public void Delete(int id)
        {
        }
    }
}
