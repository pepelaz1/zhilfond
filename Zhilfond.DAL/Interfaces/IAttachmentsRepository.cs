using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IAttachmentsRepository
    {
        IEnumerable<Attachment> Get(int id_house);
        Attachment GetById(int id);
        void Add(Attachment item);
        void Update(int id, Attachment item);
        void Delete(int id);
    }
}
