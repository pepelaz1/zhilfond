using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UI.Controllers
{
    public class MatchesApiController : ApiController
    {
        public dynamic Get(string sidx, string sord, int page, int rows)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    var matches = (from flds in ctx.Fields
                                   join f in ctx.Forms on flds.Id_form equals f.Id
                                   select new { key = "[" + f.Title + "].[" + flds.Title + "]" }
                                                                        );
                    int n = 1;
                    matches = matches.Distinct().OrderBy(x => x.key);
                    List<MatchRow> matchesRows = new List<MatchRow>();
                    foreach (var m in matches)
                    {
                        matchesRows.Add(new MatchRow() { i = (n++).ToString(), key = m.key });
                    }

                    var res = new
                    {
                        total = 1,
                        page = 1,
                        records = matches.Count(),
                        rows = (from m in matchesRows
                                    select new
                                    {
                                        i = m.i,
                                        cell = new string[] 
                                        {
                                           m.key
                                        }
                                    }).ToArray()
                    };
                    return res;
                }
                catch (Exception)
                {                    
                    return new List<string>();
                }
            }
        }
    }

    public class MatchRow
    {
        public string i;
        public string key;
    }
}
