using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UI.Controllers
{
    public class KeysApiController : ApiController
    {
        static readonly IUsersRepository _rep_users = new UsersRepository();
        static readonly IPrivateKeysRepository _priv_repository = new PrivateKeysRepository();
        static readonly IKeysRepository _repository = new KeysRepository();
             
        
        // GET api/keysapi/
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                var keys = _repository.GetAll(Request.GetQueryNameValuePairs());

                var pageIndex = Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = keys.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                keys = keys.Skip(pageIndex * pageSize).Take(pageSize);

                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from k in keys
                        select new
                        {
                            i = k.Id.ToString(),
                            cell = new string[] 
                            {
                               k.Id.ToString(),
                               k.Date_Start.ToString("dd.MM.yyyy HH:mm:ss"),
                               k.Date_Finish.ToString("dd.MM.yyyy HH:mm:ss"),
                               k.KeyValue                               
                            }
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     

        // POST api/keysapi/5
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;                

                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_user")
                        ((dynamic)jsonData).Id_User = int.Parse(pair.Value);
                }
                
                dynamic json = jsonData;
                string token = json.Token;
                string key = json.PublicKey;
                string priv_key = json.PrivateKey;
                User user = _rep_users.GetByToken(token);

                _repository.Add(new Key()
                {
                    Id_User = user.Id,
                    KeyValue = key,
                    Date_Start = DateTime.Now,
                    Date_Finish = DateTime.Now.AddYears(1)
                });

                if (priv_key != string.Empty && priv_key != null)
                {
                    _priv_repository.Add(new PrivateKey()
                    {
                        Id_user = user.Id,
                        KeyValue = priv_key
                    });
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/roleshousesapi/5
        
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                foreach (var pair in Request.GetQueryNameValuePairs())
                {
                    if (pair.Key == "id_user")
                        json.Id_User = int.Parse(pair.Value);
                }
                
                string key = json.PublicKey;
                DateTime date_start = DateTime.Parse((string)json.Date_Start);
                DateTime date_finish = DateTime.Parse((string)json.Date_Finish);
                User user = _rep_users.Get((int)json.Id_User);
                Key tempKey = new Key()
                {
                    Id_User = user.Id,
                    KeyValue = key,
                    Date_Start = date_start,
                    Date_Finish = date_finish
                };


                _repository.Update(id, tempKey);


                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        
        public dynamic Get()
        {
            try
            {
                string login = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "login"
                                select x.Value).FirstOrDefault();

                return _repository.GetByLogin(login);
            }
            catch (Exception ex)
            {
                return new Key();
            }
        }

    }
}
