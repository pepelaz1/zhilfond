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
    public class UmessagesRepository : IUmessagesRepository
    {
        static readonly IUsersRepository _users_repository = new UsersRepository();
        
        public IEnumerable<UnreadMessageV> Get(IEnumerable<KeyValuePair<string, string>> map)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    string token = "";
                    foreach (var pair in map)
                    {
                        if (pair.Key == "token")
                            token = pair.Value;
                    }

                    var user = _users_repository.GetByToken(token);
                    var result = new List<UnreadMessageV>();
                    foreach (var r in (from m in ctx.Messages
                                       join u in ctx.Users on m.Id_user equals u.Id
                                       join um in ctx.UnreadMessages on m.Id equals um.Id_message
                                       where um.Id_user == user.Id
                                       orderby m.Created descending
                                       select new UnreadMessageV
                                       {
                                           Id = (int)um.Id,
                                           Id_house = m.Id_house,
                                           Login = u.Login,
                                           WhenDateTime = m.Created,
                                           Text = m.Text
                                       }))
                    {
                      //  r.Baloon = Utils.ResolveBaloonTemplate(r.Id_house);
                        result.Add(r);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }        
            }
        }


        public int GetCount(int id_user)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    return (from x in ctx.UnreadMessages
                            where x.Id_user == id_user
                            select x).Count();

                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }


        public void Delete(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    var um = new UnreadMessage { Id = id };
                    ctx.UnreadMessages.Attach(um);
                    ctx.UnreadMessages.Remove(um);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
