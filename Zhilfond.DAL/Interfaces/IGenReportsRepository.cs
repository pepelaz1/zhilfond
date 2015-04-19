using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IGenReportsRepository
    {
        GenReport Add(GenReport item);
        IEnumerable<GenReport> Get(int id_user);
        GenReport GetById(int id);
        IEnumerable<GenReport> GetByRoleId(int role_id);
        GenReport GetByContent(byte[] content);
        bool Validate(byte[] content);
    }
}
