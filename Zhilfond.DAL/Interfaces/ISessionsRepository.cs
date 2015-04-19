using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface ISessionsRepository
    {
        void Put(string token, int id_user);
        Session GetByToken(string token);
    }
}
