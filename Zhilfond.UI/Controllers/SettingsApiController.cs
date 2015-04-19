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
    public class SettingsApiController : ApiController
    {
         static readonly ISettingsRepository _repository = new SettingsRepository();

         // GET api/settingsapi
         public Settings Get()
         {
             try
             {
                 return _repository.Get();
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

          

         // POST api/settingsapi
         public HttpResponseMessage Post(JObject jsonData)
         {
             try
             {
                 Settings settings = jsonData.ToObject<Settings>();
                 _repository.Update(settings);
                   var response = Request.CreateResponse<Coords>(HttpStatusCode.Created, null);
                 string uri = Url.Link("DefaultApi", null);
                 response.Headers.Location = new Uri(uri);
                 return response;
             }
             catch
             {
                 return new HttpResponseMessage(HttpStatusCode.InternalServerError);
             }
         }


    }
}
