using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IGroupsRepository
    {
        IEnumerable<Group> Get(int id_user);
        void Add(Group item);
        void Update(int id, Group item);
        void Delete(int id);
    }
}
