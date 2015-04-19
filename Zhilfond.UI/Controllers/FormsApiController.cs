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

namespace UI.Controllers
{
    public class FormsApiController : ApiController
    {
        static readonly IFormsRepository _repository = new FormsRepository();
        static readonly IUsersRepository _users_repository = new UsersRepository();

        // GET api/formsapi
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            try
            {
                IEnumerable<string> col = from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "type"
                                          select x.Value;
                if (col.Count() > 0)
                {
                    string type = col.FirstOrDefault();
                    if (type == "plain")
                    {
                        string token = (from x in Request.GetQueryNameValuePairs()
                                        where x.Key == "token"
                                        select x.Value).First();
                        UserV u = _users_repository.GetVByToken(token);
                        return _repository.GetAll(u);
                    }
                }

                String searchField = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchField").Value;
                String searchOper = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchOper").Value;
                String searchString = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchString").Value;
              

                var forms = _repository.GetAll(sidx, sord, searchField, searchOper, searchString);

                var pageIndex = Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = forms.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                forms = forms.Skip(pageIndex * pageSize).Take(pageSize);
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from form in forms
                        select new
                        {
                            i = form.Id.ToString(),
                            cell = new string[] 
                            {
                               form.Id.ToString(),
                               form.Order.ToString(),
                               form.Title,
                            }
                        }).ToArray()
                };
            }
            catch (Exception)
            {
                return new List<ZForm>();
            }
        }

        // GET api/formsapi/
        public dynamic Get()
        {
            try
            {

                string token = (from x in Request.GetQueryNameValuePairs()
                                where x.Key == "token"
                                select x.Value).First();

                UserV u = _users_repository.GetVByToken(token);
                var forms = _repository.GetAll(u);
                return new
                {
                    rows = (
                    from form in forms
                    select new
                    {
                        form.Id,
                        form.Order,
                        form.Title

                    }).ToArray()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET api/formsapi/5
        public ZForm Get(int id)
        {
            return null;
        }

        // POST api/formsapi
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                ((dynamic)jsonData).id = -1;
                ZForm form = jsonData.ToObject<ZForm>();
                _repository.Add(form);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/formsapi/5
        public HttpResponseMessage Put(int id, JObject jsonData)
        {
            try
            {
                ZForm form = jsonData.ToObject<ZForm>();
                _repository.Update(id, form);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
              return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/formsapi/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
