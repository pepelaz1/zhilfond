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
    public class ValidateApiController : ApiController
    {
        static readonly IImpFilesRepository _repository = new ImpFilesRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();
        static readonly IGenReportsRepository _genrep_repository = new GenReportsRepository();

      
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
                    MultipartFormDataParser parser = new MultipartFormDataParser(task.Result);
                    foreach (var f in parser.Files)
                    {
                        using (var r = new StreamReader(f.Data))
                        {
                            long len = r.BaseStream.Length;
                            byte []buff = new byte[len];
                            r.BaseStream.Read(buff, 0, (int)len);

                            bool result = _genrep_repository.Validate(buff);

                            var response = Request.CreateResponse<Object>(HttpStatusCode.OK, result);
                            string uri = Url.Link("DefaultApi", new { id = -1 });
                            response.Headers.Location = new Uri(uri);

                            return response;                            
                        }
                    }    

                    var resp = Request.CreateResponse<Object>(HttpStatusCode.OK, false);
                    string s = Url.Link("DefaultApi", new { id = -1 });
                    resp.Headers.Location = new Uri(s);
                    return resp;

                }
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }                      
        }

      
    }
}
