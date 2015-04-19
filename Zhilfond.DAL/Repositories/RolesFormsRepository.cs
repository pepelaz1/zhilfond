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
    public class RolesFormsRepository : IRolesFormsRepository
    {
        public IEnumerable<RoleFormV> Get(int id_role)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    List<RoleFormV> result = new List<RoleFormV>();
                    foreach (var r in (from rf in ctx.RolesForms
                                       join r in ctx.Roles on rf.Id_Role equals r.Id
                                       join f in ctx.Forms on rf.Id_Form equals f.Id
                                       join c in ctx.Categories on rf.Id_Cat equals c.Id
                                       join rt in ctx.Rights on rf.Id_Right equals rt.Id
                                       where r.Id == id_role
                                       select new RoleFormV
                                       {
                                           Id = rf.Id,
                                           Id_Role = r.Id,
                                           Id_Form = f.Id,
                                           Id_Cat = c.Id,
                                           Id_Right = rt.Id,
                                           Role = r.Title,
                                           Form = f.Title,
                                           Cat = c.Title,
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

        public RoleForm Add(RoleForm item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.RolesForms.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return item;
        }

        public bool Update(int id, RoleForm item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    var roleform = ctx.RolesForms.Single(p => p.Id == id);

                    roleform.Id_Role = item.Id_Role;
                    roleform.Id_Cat = item.Id_Cat;
                    roleform.Id_Form = item.Id_Form;
                    roleform.Id_Right = item.Id_Right;

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
                var roleform = ctx.RolesForms.Single(p => p.Id == id);

                ctx.RolesForms.Remove(roleform);
                ctx.SaveChanges();
            }
        }
    }
}
