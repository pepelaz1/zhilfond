using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IFormulasRepository
    {
        IEnumerable<Formula> Get(int id_field);
        void Add(Formula item);
        void Update(int id, Formula item);
        void Delete(int id);
    }
}
