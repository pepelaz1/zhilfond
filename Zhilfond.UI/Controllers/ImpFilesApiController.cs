using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Newtonsoft.Json.Linq;
using UI.Classes;

namespace UI.Controllers
{
    public class ImpFilesApiController : ApiController
    {
        static readonly IImpFilesRepository _repository = new ImpFilesRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();

        // GET api/impfilespi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
         
                string token = (from x in Request.GetQueryNameValuePairs()
                                   where x.Key == "token"
                                   select x.Value).FirstOrDefault();
                
                User user = _users_repository.GetByToken(token);
                

                var impfiles = _repository.Get(user.Id);

                int n = 0;
                return new
                {
                    total = 1,
                    page = 1,
                    records = impfiles.Count(),
                    rows = (
                        from imf in impfiles
                        select new
                        {
                            i = (n++).ToString(),
                            cell = new string[] 
                            {
                                imf.Id.ToString(),
                                imf.Filename,
                                imf.Created.ToString("dd.MM.yyyy HH:mm:ss"),
                                imf.Username
                            }
                        }).ToArray()
                };
            }
            catch (Exception )
            {
                return new List<ImpFile>();
            }
        }
    }
}
