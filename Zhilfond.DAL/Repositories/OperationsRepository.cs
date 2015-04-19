using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Security.Cryptography;


namespace DAL.Repositories
{
    public class OperationsRepository : IOperationsRepository
    {
        public IEnumerable<RuleOperation> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<RuleOperation> result = new List<RuleOperation>();
                foreach (var r in (from x in ctx.RuleOperations
                                   orderby x.Operation
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }      
    }
}
