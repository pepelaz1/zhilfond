using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IGroupHousesRepository
    {
        IEnumerable<GroupHouseV> Get(int id_form);
        void Add(GroupHouse item);
        void Update(int id, GroupHouse item);
        void Delete(int id);
    }
}
