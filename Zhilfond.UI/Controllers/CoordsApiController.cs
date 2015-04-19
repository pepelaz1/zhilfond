using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Models;
using UI.Classes;
using UI.Filters;
using DAL.Interfaces;
using DAL.Repositories;
using Newtonsoft.Json.Linq;

namespace UI.Controllers
{
    public class CoordsApiController : ApiController
    {
        static readonly ICoordsRepository _repository = new CoordsRepository();


        // GET api/coordsapi/
        public dynamic Get()
        {
            try
            {
                int id_house = int.Parse((from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "id_house"
                                          select x.Value).First().ToString());

                return _repository.Get(id_house);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST api/coordsapi/
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                Coords coords = jsonData.ToObject<Coords>();
                _repository.Update(coords);
                var response = Request.CreateResponse<Coords>(HttpStatusCode.Created, null);
                string uri = Url.Link("DefaultApi", null);
                response.Headers.Location = new Uri(uri);
                return response;

                //return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


    }
}
