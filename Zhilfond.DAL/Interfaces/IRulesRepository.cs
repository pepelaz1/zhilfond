using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRulesRepository
    {
        IEnumerable<RuleV> Get(int id_field);
        void Add(Rule item);
        void Update(int id, Rule item);
        void Delete(int id);
    }
}
