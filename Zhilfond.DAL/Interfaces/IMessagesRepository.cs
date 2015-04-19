using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IMessagesRepository
    {
        IEnumerable<MessageV> Get(int id_house, int id_user);
        void Add(Message item);
    }
}
