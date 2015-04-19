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


namespace DAL.Repositories
{
    public class DictsValuesRepository : IDictsValuesRepository
    {

        public IEnumerable<DictValue> Get(IEnumerable<KeyValuePair<string, string>> map)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    string id_dict = (from x in map
                                      where x.Key == "id_dict"
                                      select x.Value).FirstOrDefault();
                                        
                    var result = new List<DictValue>();
                    string query = @"select dv.Id, dv.Id_dict, dv.Value
                                     from UserContext.DictsValues as dv 
                                     where dv.Id_dict = " + id_dict;

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
                            query += " and cast(dv.Id as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "Value")
                            query += " and tolower(cast(dv.Value as system.string)) like '%" + pair.Value.ToLower() + "%'";         
                    }


                    if (!string.IsNullOrEmpty(srfield))
                    {
                        query += " where dv." + srfield;
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
                        query += " ORDER BY dv." + sidx + " " + sord;
                    else
                        query += " ORDER BY dv.Id";


                    ObjectQuery<DbDataRecord> records = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DbDataRecord>(query);
                    foreach (var r in records)
                    {
                        result.Add(new DictValue()
                        {
                            Id = int.Parse(r[0].ToString()),
                            Id_dict = int.Parse(r[1].ToString()),
                            Value = r[2].ToString()
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

        public IEnumerable<DictValue> GetAll()
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
        }

    }
}
