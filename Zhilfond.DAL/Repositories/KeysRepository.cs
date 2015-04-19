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
    public class KeysRepository : IKeysRepository
    {
        static readonly IUsersRepository _rep_users = new UsersRepository();

        public IEnumerable<Key> GetAll(IEnumerable<KeyValuePair<string, string>> map)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    string id_user = (from x in map
                                      where x.Key == "id_user"
                                      select x.Value).FirstOrDefault();

                    var result = new List<Key>();
                    string query = @"select k.Id, k.Id_User, k.KeyValue, k.Date_Start, k.Date_Finish
                                     from UserContext.Keys as k 
                                     where k.Id_User = " + id_user;

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
                            query += " and cast(k.Id as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "Value")
                            query += " and cast(k.KeyValue as system.string) like '" + pair.Value + "%'";
                    }


                    if (!string.IsNullOrEmpty(srfield))
                    {
                        query += " where k." + srfield;
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
                        query += " ORDER BY k." + sidx + " " + sord;


                    ObjectQuery<DbDataRecord> records = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DbDataRecord>(query);
                    foreach (var r in records)
                    {
                        result.Add(new Key()
                        {
                            Id = int.Parse(r[0].ToString()),
                            Id_User = int.Parse(r[1].ToString()),
                            KeyValue = r[2].ToString(),
                            Date_Start = DateTime.Parse(r[3].ToString()),
                            Date_Finish = DateTime.Parse(r[4].ToString())
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

        public IEnumerable<Key> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<Key> result = new List<Key>();
                foreach (var r in (from x in ctx.Keys
                                   orderby x.Id_User
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public IEnumerable<Key> GetByUser(int id_user)
        {
            using (var ctx = new UserContext())
            {
                List<Key> result = new List<Key>();
                foreach (var r in (from x in ctx.Keys
                                   where x.Id_User == id_user        
                                   orderby x.Date_Start descending
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public Key GetByToken(string token)
        {
            User u = _rep_users.GetByToken(token);
            using (var ctx = new UserContext())
            {
                return (from x in ctx.Keys
                        where x.Id_User == u.Id
                        orderby x.Date_Start descending
                        select x).FirstOrDefault();
            }
        }


        public Key Get(int id)       
        {
            using (var ctx = new UserContext())
            {
                return ctx.Keys.Single(p => p.Id == id);
            }
        }

        public Key GetByLogin(string login)
        {
            using (var ctx = new UserContext())
            {
                return (from x in ctx.Keys
                        join u in ctx.Users on x.Id_User equals u.Id
                        where u.Login == login
                        orderby x.Date_Start descending
                        select x).FirstOrDefault();
            }
        }

        public Key Add(Key item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Keys.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return item;
        }

        public bool Update(int id, Key item)             
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        Key f = (from x in ctx.Keys
                                        where x.Id == id
                                        select x).First();

                        //.KeyValue = item.KeyValue;
                        //f.Id_User = item.Id_User;
                        f.Date_Start = item.Date_Start;
                        f.Date_Finish = item.Date_Finish;

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
                        var f = new Key { Id = id };
                        ctx.Keys.Attach(f);
                        ctx.Keys.Remove(f);
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
