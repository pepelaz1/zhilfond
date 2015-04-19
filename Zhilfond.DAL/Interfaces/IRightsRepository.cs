using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRightsRepository
    {
        IEnumerable<Right> GetAll();
        IEnumerable<Right> GetAllDesc();
        Right Get(int id);
        Right Add(Right item);
        bool Update(int id, Right item);
        bool Delete(int id);        
    }
}
