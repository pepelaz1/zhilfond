using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRolesHousesRepository
    {
        IEnumerable<RoleHouseV> Get(int id_role);        
        RoleHouse Add(RoleHouse item);
        bool Update(int id, RoleHouse item);
        void Delete(int id);        
    }
}
