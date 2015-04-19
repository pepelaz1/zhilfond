using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using ClosedXML.Excel;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using UI.Classes;

namespace UI.Controllers
{
    public class XmlGeneratorApiController : ApiController
    {
        static readonly IUsersRepository _users_repository = new UsersRepository();
        static readonly IGenReportsRepository _genrep_repository = new GenReportsRepository();
        static readonly IXmlRepository _repository = new XmlRepository();

        // GET api/xmlgeneratorapi
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
                
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                string data = _repository.Generate(id_group);
                result.Content = new StringContent(data); 
   
                //string filename = Guid.NewGuid().ToString() + ".xml";
                string filename = "data.xml";

                ////////////// Сохраняем в базу генериный отчет ///////////////////
                var gr = new GenReport()
                {
                    Id_user = u.Id,
                    Filename = filename,
                    Created = DateTime.Now,
                    Data = Encoding.UTF8.GetBytes (data),
                    Type = "xml"
                };
                _genrep_repository.Add(gr);
                ///////////////////////////////////////////////////

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filename
                };
                return result;
            }
            catch (Exception ex )
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        // GET api/xmlgeneratorapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/xmlgeneratorapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/xmlgeneratorapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/xmlgeneratorapi/5
        public void Delete(int id)
        {
        }
    }
}
