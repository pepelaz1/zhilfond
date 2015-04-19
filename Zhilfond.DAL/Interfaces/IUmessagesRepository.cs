using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IUmessagesRepository
    {
        IEnumerable<UnreadMessageV> Get(IEnumerable<KeyValuePair<string, string>> map);
        int GetCount(int id_user);
        void Delete(int id);
    }
}
