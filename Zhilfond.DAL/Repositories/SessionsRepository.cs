using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class SessionsRepository : ISessionsRepository
    {
        public Session GetByToken(string token)
        {
            using (var ctx = new UserContext())
            {
                return (from x in ctx.Sessions
                        where x.Token == token
                        select x).FirstOrDefault();
            }
        }

        public void Put(string token, int id_user)
        {
            using (var ctx = new UserContext())
            {
                ctx.Sessions.Add(new Session()
                {
                    Token = token,
                    CreatedTime = DateTime.Now,
                    UserId = id_user
                });
                ctx.SaveChanges();
            }
        }
    }
}
