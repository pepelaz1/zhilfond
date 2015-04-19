using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IFieldsRepository
    {
        IEnumerable<ZFieldV> Get(int id_form);
        IEnumerable<ZFieldV> GetDetails(int id_form);
        void Add(ZField item, int id_category);
        void Update(int id, ZField item, int id_category);
        void Delete(int id);
    }
}
