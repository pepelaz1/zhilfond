using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Security.Cryptography;
using DAL.Classes;


namespace DAL.Repositories
{
    public class ReportTemplatesRepository : IReportTemplatesRepository
    {
        public IEnumerable<ReportTemplate> Get()
        {
            using (var ctx = new UserContext())
            {
                List<ReportTemplate> result = new List<ReportTemplate>();
                foreach (var r in (from x in ctx.ReportTemplates                                   
                                   orderby x.Id
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }


        public void Add(ReportTemplate item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.ReportTemplates.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public ReportTemplate GetById(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {

                    return (from x in ctx.ReportTemplates
                            where x.Id == id
                            select x).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ReportTemplate GetByName(string sTemplateName)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    return (from x in ctx.ReportTemplates
                            where x.Filename == sTemplateName
                            select x).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool Update(int id, ReportTemplate item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        ReportTemplate f = (from x in ctx.ReportTemplates
                                  where x.Id == id
                                  select x).First();
                        f.Reportname = item.Reportname;
                        //f.Data = f.Data;
                        f.Reporttype = item.Reporttype;
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Delete(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    var f = ctx.ReportTemplates.Find(id);
                    ctx.ReportTemplates.Attach(f);
                    ctx.ReportTemplates.Remove(f);
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
