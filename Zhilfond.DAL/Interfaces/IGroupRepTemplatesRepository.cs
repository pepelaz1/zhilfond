using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Interfaces
{
    public interface IGroupRepTemplatesRepository
    {
        IEnumerable<GroupRepTemplate> GetAll();
        GroupRepTemplate Get(int id);
        GroupRepTemplate Add(GroupRepTemplate item);
        bool Update(int id, GroupRepTemplate item);
        bool Delete(int id);  
    }
}
