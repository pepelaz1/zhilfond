using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Interfaces;
using DAL.Repositories;
using UI.Classes;

namespace UI.Controllers
{
    public class HousesApiController : ApiController
    {
        static readonly IHousesRepository _repository = new HousesRepository();

        // GET api/housesapi
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
                        return _repository.GetAllIds();
                    }
                }


              // String searchField = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchField").Value;
               // String searchOper = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchOper").Value;
              //  String searchString = Request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "searchString").Value;

                var housesv = _repository.GetAllView(Request.GetQueryNameValuePairs());

                var pageIndex = Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = housesv.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                housesv = housesv.Skip(pageIndex * pageSize).Take(pageSize);
                return new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from housev in housesv
                        select new
                        {
                            i = housev.id.ToString(),
                            cell = new string[] 
                            {
                               housev.id.ToString(),
                               housev.nasp,
                               housev.raion,
                               housev.street,
                               housev.number.ToString(),
                               housev.letter,
                               housev.oorg,
                               housev.tdoma,
                               housev.seriya,
                               housev.date_exp == null ? null : ((DateTime)DateTime.Parse(housev.date_exp.ToString())).ToString("dd.MM.yyyy"),
                               housev.etaz_max.ToString(),
                               housev.kolmunkv.ToString(),
                               housev.ploo.ToString(),
                               housev.kolzp.ToString(),
                               housev.fupr
                      }
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public dynamic Get()
        {
            try
            {

                IEnumerable<string> col = from x in Request.GetQueryNameValuePairs()
                                          where x.Key == "type"
                                          select x.Value;
                if (col.Count() > 0)
                {
                    string type = col.FirstOrDefault();
                    if (type == "pageno")
                    {
                        int pagesize = int.Parse((from x in Request.GetQueryNameValuePairs()
                                                  where x.Key == "pagesize"
                                                  select x.Value).First().ToString());

                        int id_house = int.Parse((from x in Request.GetQueryNameValuePairs()
                                                  where x.Key == "id_house"
                                                  select x.Value).First().ToString());

                        return _repository.GetPageNo(id_house, pagesize);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // GET api/housesapi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/housesapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/housesapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/housesapi/5
        public void Delete(int id)
        {
        }
    }
}
