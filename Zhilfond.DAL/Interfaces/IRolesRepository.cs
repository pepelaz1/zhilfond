using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRolesRepository
    {
        IEnumerable<Role> GetAll();
        IEnumerable<Role> GetAllDesc();
        Role Get(int id);
        Role Add(Role item);
        bool Update(int id, Role item);
        bool Delete(int id);        
    }
}
