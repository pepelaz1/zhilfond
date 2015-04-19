using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IUsersRepository
    {
        IEnumerable<User> GetAll();
        IEnumerable<GridUser> GetAll(String sfld, String sord, String srfld, String srop, String srvalue);        
        User Get(int id);
        User Get(String username, String passwordHash);
        User GetByToken(string token);
        User GetByLogin(string token);
        UserV GetVByToken(string token);
        User Add(User item);
        void Delete(int id);
        bool Update(int id, User item);
        bool ChangePassword(ChPwd chpwd);       
    }
}
