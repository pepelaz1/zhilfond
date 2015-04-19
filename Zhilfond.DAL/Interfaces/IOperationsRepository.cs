using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IOperationsRepository
    {
        IEnumerable<RuleOperation> GetAll();
    }
}
