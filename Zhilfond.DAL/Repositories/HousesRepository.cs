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
    public class HousesRepository : IHousesRepository
    {
        static readonly IUsersRepository _users_repository = new UsersRepository();

        public IEnumerable<HouseV> GetAllView(IEnumerable<KeyValuePair<string, string>> map)
        {
           
            using (var ctx = new UserContext())
            {
                try
                {

                    string token = (from x in map
                                    where x.Key == "token"
                                    select x.Value).FirstOrDefault();

                    UserV u = _users_repository.GetVByToken(token);

                    List<HouseV> result = new List<HouseV>();

                    
                    string query = "";
                    if (u.Role == "Администраторы")
                        query = @"select h.Id, h.nasp, h.raion, h.street,h.number,h.letter,h.oorg,h.tdoma,h.seriya,h.date_exp,h.etaz_max,h.kolmunkv,h.ploo,h.kolzp,h.fupr
                                     from UserContext.HousesV as h where 1=1";
                    else
                        query = @"select h.Id, h.nasp, h.raion, h.street,h.number,h.letter,h.oorg,h.tdoma,h.seriya,h.date_exp,h.etaz_max,h.kolmunkv,h.ploo,h.kolzp,h.fupr
                                  from UserContext.HousesV as h 
                                  join UserContext.RolesHouses  as rh on h.Id = rh.Id_House
                                  join UserContext.Roles as r on rh.Id_Role = r.Id and r.Title ='" + u.Role + @"' where 1=1";

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
                            query += " and cast(h.Id as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "nasp")
                            query += " and cast(h.nasp as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "raion")
                            query += " and cast(h.raion as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "street")
                            query += " and cast(h.street as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "number")
                            query += " and cast(h.number as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "letter")
                            query += " and cast(h.letter as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "oorg")
                            query += " and cast(h.oorg as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "tdoma")
                            query += " and cast(h.tdoma as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "seriya")
                            query += " and cast(h.seriya as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "date_exp")
                            query += " and cast(h.date_exp as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "etaz_max")
                            query += " and cast(h.etaz_max as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "kolmunkv")
                            query += " and cast(h.kolmunkv as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "ploo")
                            query += " and cast(h.ploo as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "kolzp")
                            query += " and cast(h.kolzp as system.string) like '" + pair.Value + "%'";
                        else if (pair.Key == "fupr")
                            query += " and cast(h.fupr as system.string) like '" + pair.Value + "%'";
                    }


                    if (!string.IsNullOrEmpty(srfield))
                    {
                        query += " where h." + srfield;
                        if (srop == "eq")
                            query += "=" + srvalue;
                        else if (srop == "neq")
                            query += "<>" + srvalue;
                        else if (srop == "cn")
                            query += " like '%" + srvalue +"%'";
                        else if (srop == "ncn")
                            query += " not like '%" + srvalue +"%'";
                    }


                    if (!string.IsNullOrEmpty(sidx))
                        query += " ORDER BY h." + sidx + " " + sord;
                    

                    ObjectQuery<DbDataRecord> records = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DbDataRecord>(query);             
                    foreach (var r in records)
                    {
                        result.Add( new HouseV() 
                            {
                                id = int.Parse(r[0].ToString()),
                                nasp = r[1].ToString(),
                                raion = r[2].ToString(),
                                street = r[3].ToString(),
                                number = r[4].ToString() == "" ? null : (int?)int.Parse(r[4].ToString()), 
                                letter = r[5].ToString(),
                                oorg = r[6].ToString(),
                                tdoma = r[7].ToString(),
                                seriya = r[8].ToString(),
                                date_exp = r[9].ToString() == "" ? null : (DateTime?)DateTime.Parse(r[9].ToString()),
                                etaz_max = r[10].ToString() == "" ? null : (int?)int.Parse(r[10].ToString()),
                                kolmunkv = r[11].ToString() == "" ? null : (int?)int.Parse(r[11].ToString()),
                                ploo = r[12].ToString() == "" ? null : (int?)int.Parse(r[12].ToString()),
                                kolzp = r[13].ToString() == "" ? null : (int?)int.Parse(r[13].ToString()),
                                fupr = r[14].ToString()
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

        public IEnumerable<House> GetAllIds()
        {
            using (var ctx = new UserContext())
            {
                List<House> result = new List<House>();
                foreach (var r in (from x in ctx.Houses
                                   orderby x.Id
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public int GetPageNo(int id_house, int pagesize)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                   // ctx.Houses.Count()
                    return 1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
