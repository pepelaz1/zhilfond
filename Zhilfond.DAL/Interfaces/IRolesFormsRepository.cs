using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRolesFormsRepository
    {
        IEnumerable<RoleFormV> Get(int id_role);
        RoleForm Add(RoleForm item);
        bool Update(int id, RoleForm item);
        void Delete(int id);        
    }
}
