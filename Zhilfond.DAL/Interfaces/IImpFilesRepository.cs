using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IImpFilesRepository
    {
        IEnumerable<ImpFileV> Get(int id_user);
        ImpFile Add(ImpFile item);
        void SignFile(int fileId);
    }
}
