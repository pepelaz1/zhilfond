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
    public class ImpDetailsApiController : ApiController
    {
        static readonly IResultsRepository _repository = new ResultsRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();

        // GET api/impdetails
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {         
                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).FirstOrDefault();
                
                int id_import = int.Parse((from x in Request.GetQueryNameValuePairs()
                                           where x.Key == "id_import"
                                           select x.Value).FirstOrDefault().ToString());
                
                var user = _users_repository.GetByToken(token);               
                var details = _repository.GetImportDetails(user, id_import);

                int n = 0;
                return new
                {
                    total = 1,
                    page = 1,
                    records = details.Count(),
                    rows = (
                        from imd in details
                        select new
                        {
                            i = (n++).ToString(),
                            cell = new string[]
                            {
                                imd.Id.ToString(),
                                imd.Status,
                                imd.House,
                                imd.Element,
                                imd.Error
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
