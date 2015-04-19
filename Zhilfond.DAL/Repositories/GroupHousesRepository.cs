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
    public class GroupHousesRepository : IGroupHousesRepository
    {
        public IEnumerable<GroupHouseV> Get(int id_group)
        {
            try
            {
               using (var ctx = new UserContext())
                {
                    List<GroupHouseV> result = new List<GroupHouseV>();
                    foreach (var r in (from gh in ctx.GroupHouses
                                       join h in ctx.HousesV on gh.Id_house equals h.id
                                       where gh.Id_group == id_group
                                       orderby gh.Id
                                       select new GroupHouseV
                                       {
                                           Id = gh.Id,
                                           Id_group = gh.Id_group,
                                           Street = h.street,
                                           Number = h.number,
                                           Letter = h.letter,
                                           Id_house = h.id,
                                           Latitude = h.latitude,
                                           Longitude = h.longitude                                          
                                       }))
                    {
                        r.Baloon = Utils.ResolveBaloonTemplate(r.Id_house);
                        result.Add(r);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new List<GroupHouseV>();
            }
        }
           

        public void Add(GroupHouse item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    GroupHouse gh = ctx.GroupHouses.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Update(int id, GroupHouse item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                   // GroupHouse f = (from x in ctx.GroupHouses
                   //                 where x.Id == id
                  //                  select x).First();
                    //f.Title = item.Title;
                   // f.Id_dict = item.Id_dict;
                    //f.Order = item.Order;
                    //f.Required = item.Required;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    var f = new GroupHouse { Id = id };
                    ctx.GroupHouses.Attach(f);
                    ctx.GroupHouses.Remove(f);
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
