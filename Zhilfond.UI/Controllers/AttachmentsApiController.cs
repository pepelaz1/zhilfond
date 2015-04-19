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
    public class AttachmentsApiController : ApiController
    {
        static readonly IAttachmentsRepository _repository = new AttachmentsRepository();

        // GET api/attachmentsapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                int id_house = int.Parse((from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "id_house" 
                                          select x.Value).FirstOrDefault().ToString());

                var attachments = _repository.Get(id_house);
                return new
                {
                    total = 1,
                    page = 1,
                    records = attachments.Count(),
                    rows = (
                        from attachment in attachments
                        select new
                        {
                            i = attachment.Id.ToString(),
                            cell = new string[] 
                            {
                                attachment.Id.ToString(),
                                attachment.Order.ToString(),
                                attachment.Filename
                            }
                        }).ToArray()
                };
             
            }
            catch (Exception ex)
            {
                return new List<Attachment>();
            }
        }

        // GET api/attachmentsapi/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                Attachment a = _repository.GetById(id);
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
                    int id_house = int.Parse((from x in Request.GetQueryNameValuePairs()
                                              where x.Key == "id_house"
                                              select x.Value).FirstOrDefault().ToString());

                    var task = Request.Content.ReadAsStreamAsync();
                    task.Wait();

                    List<string> files = new List<string>();
                    MultipartFormDataParser parser = new MultipartFormDataParser(task.Result);
                    foreach (var f in parser.Files)
                    {
                        using (var stream = new StreamReader(f.Data))
                        {
                            
                            var a = new Attachment()
                            {
                                Id_house = id_house,
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


        // PUT api/attachmentsapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                Attachment a = jsonData.ToObject<Attachment>();
                _repository.Update(id, a);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/attachmentsapi/5
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
