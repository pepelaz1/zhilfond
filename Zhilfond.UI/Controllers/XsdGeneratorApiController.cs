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
    public class XsdGeneratorApiController : ApiController
    {
        static readonly IUsersRepository _users_repository = new UsersRepository();
        static readonly IXsdRepository _repository = new XsdRepository();
        static readonly IGenReportsRepository _genrep_repository = new GenReportsRepository();

        // GET api/xsdgeneratorapi
        public HttpResponseMessage Get()
        {
            try
            {
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();

                var u = _users_repository.GetByToken(token);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                string data = _repository.Generate();
                result.Content = new StringContent(data); 
   
                //string filename = Guid.NewGuid().ToString() + ".xsd";
                string filename = "data.xsd";
                              

                var gr = new GenReport()
                {
                    Id_user = u.Id,
                    Filename = filename,
                    Created = DateTime.Now,
                    Data = Encoding.UTF8.GetBytes(data),
                    Type = "xsd"
                };
                _genrep_repository.Add(gr);

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filename
                };
                return result;
            }
            catch (Exception )
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        // GET api/xsdgeneratorapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/xsdgeneratorapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/xsdgeneratorapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/xsdgeneratorapi/5
        public void Delete(int id)
        {
        }
    }
}
