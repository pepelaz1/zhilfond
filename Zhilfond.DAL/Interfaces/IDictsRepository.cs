using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IDictsRepository
    {
        IEnumerable<Dict> GetAll();
        IEnumerable<Dict> GetAllDesc();
        Dict Get(int id);
        Dict Add(Dict item);
        bool Update(int id, Dict item);
        bool Delete(int id);        
    }
}
