using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UI.Filters;
using Newtonsoft.Json.Linq;
using DAL.Models;
using UI.Classes;
using DAL.Interfaces;
using DAL.Repositories;

namespace UI.Controllers
{
    public class ChangePassApiController : ApiController
    {
        static readonly IUsersRepository _repository = new UsersRepository();
        #region Post
       
        public HttpResponseMessage Post(JObject jsonData)
        {
            dynamic json = jsonData;
            JObject jchpwd = json.ChPwd;
            string token = json.UserToken;
            var chpwd = jchpwd.ToObject<ChPwd>();

            if (chpwd.NewPass != chpwd.Repeat)
                return new HttpResponseMessage(HttpStatusCode.Conflict);

            User u = _repository.GetByToken(token);

            chpwd.UserId = u.Id;
            if (_repository.ChangePassword(chpwd) == false)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);


            var response = Request.CreateResponse<ChPwd>(HttpStatusCode.Created, chpwd);
            string uri = Url.Link("DefaultApi", new { id = chpwd.UserId });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        #endregion
    }
}
