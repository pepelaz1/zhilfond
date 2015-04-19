using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IResultsRepository
    {
        IEnumerable<Result> GetImportDetails(User user, int id_import);
        void Add(Result item);
    }
}
