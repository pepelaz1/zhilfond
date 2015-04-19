using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IKeysRepository
    {
        IEnumerable<Key> GetAll(IEnumerable<KeyValuePair<string, string>> map);
        IEnumerable<Key> GetAll();
        IEnumerable<Key> GetByUser(int id_user);
        Key Get(int id);
        Key GetByToken(string token);
        Key GetByLogin(string login);
        Key Add(Key item);
        bool Update(int id, Key item);
        bool Delete(int id);        
    }
}
