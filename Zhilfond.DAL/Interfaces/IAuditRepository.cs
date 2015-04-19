using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IAuditRepository
    {
        IEnumerable<AuditV> Get(IEnumerable<KeyValuePair<string, string>> map);
    }
}
