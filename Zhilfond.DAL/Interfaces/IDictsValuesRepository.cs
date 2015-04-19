using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IDictsValuesRepository
    {
        IEnumerable<DictValue> Get(IEnumerable<KeyValuePair<string, string>> map);
        IEnumerable<DictValue> GetAll();
        IEnumerable<DictValue> GetByDict(int id_dict);
        DictValue Get(int id);
        DictValue Add(DictValue item);
        bool Update(int id, DictValue item);
        bool Delete(int id);        
    }
}
