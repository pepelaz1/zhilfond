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
using System.Globalization;


namespace DAL.Repositories
{
    public class AuditRepository : IAuditRepository
    {

        public IEnumerable<AuditV> Get(IEnumerable<KeyValuePair<string, string>> map)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    var result = new List<AuditV>();
                    string query = @"select a.Id, a.Id_house, a.Login, a.Form, a.Field, a.OldVal, a.NewVal, a.whendatetime
                                     from UserContext.AuditsV as a where 1 = 1 ";

                    string srfield = "";
                    string srop = "";
                    string srvalue = "";
                    string sidx = "";
                    string sord = "";

                    foreach (var pair in map)
                    {
                        if (pair.Key == "searchField")
                            srfield = pair.Value;
                        else if (pair.Key == "searchOper")
                            srop = pair.Value;
                        else if (pair.Key == "searchString")
                            srvalue = pair.Value;
                        else if (pair.Key == "sidx")
                            sidx = pair.Value;
                        else if (pair.Key == "sord")
                            sord = pair.Value;
                        else if (pair.Key == "Id")
                            query += " and cast(a.Id as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "Id_house")
                            query += " and cast(a.Id_house as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "Login")
                            query += " and cast(a.Login as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "Form")
                            query += " and cast(a.form as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "Field")
                            query += " and cast(a.field as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "OldVal")
                            query += " and cast(a.OldVal as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "NewVal")
                            query += " and cast(a.NewVal as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "WhenDateTime")
                            query += " and cast(a.whendatetime as system.string) like '%" + pair.Value + "%'";  
                    }


                    if (!string.IsNullOrEmpty(srfield))
                    {
                        query += " where a." + srfield;
                        if (srop == "eq")
                            query += "=" + srvalue;
                        else if (srop == "neq")
                            query += "<>" + srvalue;
                        else if (srop == "cn")
                            query += " like '%" + srvalue + "%'";
                        else if (srop == "ncn")
                            query += " not like '%" + srvalue + "%'";
                    }

                    if (!string.IsNullOrEmpty(sidx))
                        query += " ORDER BY a." + sidx + " " + sord;
                    else
                        query += " ORDER BY a.whendatetime DESC";


                    ObjectQuery<DbDataRecord> records = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DbDataRecord>(query);
                    foreach (var r in records)
                    {
                        result.Add(new AuditV()
                        {
                            Id = int.Parse(r[0].ToString()),
                            Id_house = int.Parse(r[1].ToString()),
                            Login = r[2].ToString(),
                            form = r[3].ToString(),
                            field = r[4].ToString(),
                            OldVal = r[5].ToString(),
                            NewVal = r[6].ToString(),
                            whendatetime = (DateTime)r[7]
                        });
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

/*        public IEnumerable<DictValue> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<DictValue> result = new List<DictValue>();
                foreach (var r in (from x in ctx.DictsValues
                                   orderby x.Value
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public IEnumerable<DictValue> GetByDict(int id_dict)
        {
            using (var ctx = new UserContext())
            {
                List<DictValue> result = new List<DictValue>();
                foreach (var r in (from x in ctx.DictsValues
                                   where x.Id_dict == id_dict
                                   orderby x.Value
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public DictValue Get(int id)       
        {
            using (var ctx = new UserContext())
            {
                return ctx.DictsValues.Single(p => p.Id == id);
            }
        }

        public DictValue Add(DictValue item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.DictsValues.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return item;
        }

        public bool Update(int id, DictValue item)             
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        DictValue f = (from x in ctx.DictsValues
                                        where x.Id == id
                                        select x).First();
                        f.Value = item.Value;
                        f.Id_dict = item.Id_dict;
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


        public bool Delete(int id)            
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        var f = new DictValue { Id = id };
                        ctx.DictsValues.Attach(f);
                        ctx.DictsValues.Remove(f);
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
        }*/

    }
}
