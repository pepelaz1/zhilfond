using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IFormsRepository
    {
        IEnumerable<ZForm> GetAll(string sfld, string sord, string srfld, string srop, string srvalue);
        IEnumerable<ZForm> GetAll(UserV u);
        void Add(ZForm item);
        void Update(int id, ZForm item);
        void Delete(int id);
    }
}
