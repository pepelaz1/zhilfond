using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IAccessRepository
    {
        IEnumerable<Access> Get(UserV u, string form, string category);
        IEnumerable<Access> Get(UserV u, int id_house);        
    }
}
