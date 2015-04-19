using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        public IEnumerable<Access> Get(UserV u, string form, string category)
        {
            using (var ctx = new UserContext())
            {
                List<Access> result = new List<Access>();
                if (u.Role == "Администраторы")
                {
                    result.Add(new Access() { Level = "Полный доступ" });
                }
                else
                {
                    foreach (var r in (from f in ctx.RolesForms
                                       join fr in ctx.Forms on f.Id_Form equals fr.Id
                                       join c in ctx.Categories on f.Id_Cat equals c.Id
                                       join a in ctx.Rights on f.Id_Right equals a.Id
                                       where f.Id_Role == u.Id_Role && fr.Title == form && c.Title == category
                                       select new Access { Level = a.Title}))
                    {
                        result.Add(r);
                    }
                }
                return result;
            }
        }

        public IEnumerable<Access> Get(UserV u, int id_house)
        {
            using (var ctx = new UserContext())
            {
                List<Access> result = new List<Access>();
                if (u.Role == "Администраторы")
                {
                    result.Add(new Access() { Level = "Полный доступ" });
                }
                else
                {
                    foreach (var r in (from f in ctx.RolesHouses
                                       join h in ctx.Houses on f.Id_House equals h.Id
                                       join a in ctx.Rights on f.Id_Right equals a.Id
                                       where f.Id_Role == u.Id_Role && h.Id == id_house
                                       select new Access { Level = a.Title }))
                    {
                        result.Add(r);
                    }
                }
                return result;
            }
        }
    }
}
