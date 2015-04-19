using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using HttpMultipartParser;
using UI.Classes;

namespace UI.Controllers
{
    public class FileUploadApiController : ApiController
    {
        static readonly IImpFilesRepository _repository = new ImpFilesRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();

        // GET api/fileuploadapi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/fileuploadapi/5
        public string Get(int id)
        {
            return "value";
        }

        public HttpResponseMessage Post()
        {
            try
            {
                string type = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "type"
                                select x.Value).FirstOrDefault();

                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault(); 

                if (Request.Content.IsMimeMultipartContent())
                {
                    var task = Request.Content.ReadAsStreamAsync();
                    task.Wait();

                    List<ExpandoObject> objs = new List<ExpandoObject>();
              //      List<string> files = new List<string>();
                    MultipartFormDataParser parser = new MultipartFormDataParser(task.Result);
                    foreach (var f in parser.Files)
                    {
                        using (var r = new StreamReader(f.Data))
                        {
                            //files.Add(r.ReadToEnd());
                            dynamic o = new ExpandoObject();
                            o.File = r.ReadToEnd();
                            if (type == "import")
                            {
                                User u = _users_repository.GetByToken(token);
                                var imf = _repository.Add(new ImpFile()
                                {
                                    Id_user = u.Id,
                                    Filename = f.FileName,
                                    Created = DateTime.Now
                                });
                                o.Id_import = imf.Id;
                            }
                            objs.Add(o);
                        }
                    }                                      
                    
                    string data = Request.Content.ReadAsStringAsync().Result;

                    var response = Request.CreateResponse<Object>(HttpStatusCode.OK, objs);
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

        // PUT api/fileuploadapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/fileuploadapi/5
        public void Delete(int id)
        {
        }
    }
}
