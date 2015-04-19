using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using HttpMultipartParser;
using Newtonsoft.Json.Linq;

namespace UI.Controllers
{
    public class ReportTemplatesApiController : ApiController
    {
        static readonly IReportTemplatesRepository _repository = new ReportTemplatesRepository();

        // GET api/reporttemplatesapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                var reporttemplates = _repository.Get();

                return new
                {
                    total = 1,
                    page = 1,
                    records = reporttemplates.Count(),
                    rows = (
                        from reporttemplate in reporttemplates.OrderBy(x => x.Reporttype).ThenBy(y => y.Reportname)
                        select new
                        {
                            i = reporttemplate.Id.ToString(),
                            cell = new string[] 
                            {
                                reporttemplate.Id.ToString(),
                                reporttemplate.Filename,
                                reporttemplate.Reportname,
                                reporttemplate.Reporttype.ToString(),
                                reporttemplate.Data.ToString()
                            }
                        }).ToArray()
                };

            }
            catch (Exception)
            {
                return new List<Attachment>();
            }
        }

        public dynamic Get()
        {
            int nReportType = int.Parse((from x in Request.GetQueryNameValuePairs()
                                         where x.Key == "type"
                                         select x.Value).FirstOrDefault().ToString());
            var rows = _repository.Get().Where(rt => rt.Reporttype == nReportType);
            return rows;
        }

        // GET api/reporttemplatesapi/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                ReportTemplate a = _repository.GetById(id);
                MemoryStream ms = new MemoryStream();
                ms.Write(a.Data, 0, a.Data.Length);
                ms.Position = 0;

                result.Content = new StreamContent(ms);

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = a.Filename
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }


        public HttpResponseMessage Post()
        {
            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    var task = Request.Content.ReadAsStreamAsync();
                    task.Wait();

                    List<string> files = new List<string>();
                    MultipartFormDataParser parser = new MultipartFormDataParser(task.Result);
                    foreach (var f in parser.Files)
                    {
                        using (var stream = new StreamReader(f.Data))
                        {

                            var a = new ReportTemplate()
                            {                                
                                Filename = f.FileName
                            };
                            a.Data = new byte[f.Data.Length];
                            f.Data.Read(a.Data, 0, a.Data.Length);

                            _repository.Add(a);
                        }
                    }

                    var response = Request.CreateResponse<Object>(HttpStatusCode.OK, files);
                    string uri = Url.Link("DefaultApi", new { id = -1 });
                    response.Headers.Location = new Uri(uri);

                    return response;

                }
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/reporttemplatesapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                ReportTemplate tmpl = jsonData.ToObject<ReportTemplate>();
                _repository.Update(id, tmpl);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/reporttemplatesapi/5
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
