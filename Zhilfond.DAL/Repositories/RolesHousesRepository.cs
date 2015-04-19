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
    public class RolesHousesRepository : IRolesHousesRepository
    {
        public IEnumerable<RoleHouseV> Get(int id_role)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    List<RoleHouseV> result = new List<RoleHouseV>();
                    foreach (var r in (from rh in ctx.RolesHouses
                                       join r in ctx.Roles on rh.Id_Role equals r.Id
                                       join h in ctx.HousesV on rh.Id_House equals h.id
                                       join rt in ctx.Rights on rh.Id_Right equals rt.Id
                                       where r.Id == id_role
                                       select new RoleHouseV
                                       {
                                           Id = rh.Id,
                                           Id_House = rh.Id_House,
                                           Id_Role = r.Id,
                                           Id_Right = rh.Id_Right,
                                           Role = r.Title,
                                           Right = rt.Title
                                       }))
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

        //public RoleHouseV Add(RoleHouseV item)
        //{
        //    using (var ctx = new UserContext())
        //    {
        //        try
        //        {
        //            RoleHouse newitem = new RoleHouse
        //            {
        //                Id = item.Id,
        //                Id_House = item.Id_House,
        //                Id_Role = item.Id_Role,
        //                Id_Right = item.Id_Right
        //            };

        //            ctx.RolesHouses.Add(newitem);
        //            ctx.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return item;
        //}

        public RoleHouse Add(RoleHouse item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.RolesHouses.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return item;
        }

        public bool Update(int id, RoleHouse item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    var rolehouse = ctx.RolesHouses.Single(p => p.Id == id);

                    rolehouse.Id_House = item.Id_House;
                    rolehouse.Id_Role = item.Id_Role;
                    rolehouse.Id_Right = item.Id_Right;

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public void Delete(int id)
        {
            using (var ctx = new UserContext())
            {
                var rolehouse = ctx.RolesHouses.Single(p => p.Id == id);

                ctx.RolesHouses.Remove(rolehouse);
                ctx.SaveChanges();
            }
        }
    }
}
