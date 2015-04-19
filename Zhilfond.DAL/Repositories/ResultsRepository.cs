using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class ResultsRepository : IResultsRepository
    {
        public IEnumerable<Result> GetImportDetails(User user, int id_import)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    List<Result> result = new List<Result>();
                    foreach (var r in (from res in ctx.Results
                                       join u in ctx.Users on res.Id_user equals u.Id
                                       join role in ctx.Roles on u.Id_Role equals role.Id
                                       where role.Id == user.Id_Role &&
                                            res.SourceType == "import" && res.Id_source == id_import
                                       select res))
                    {
                        result.Add(r);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add(Result item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Results.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }            
        }

    }
}
