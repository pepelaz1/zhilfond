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
using DAL.Classes;


namespace DAL.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        public IEnumerable<MessageV> Get(int id_house, int id_user)
        {
            using (var ctx = new UserContext())
            {
                List<MessageV> result = new List<MessageV>();
                foreach (var r in (from x in ctx.Messages
                                   join u in ctx.Users on x.Id_user equals u.Id
                                   join um in (from t in ctx.UnreadMessages 
                                               where t.Id_user == id_user
                                               select t) on  x.Id equals um.Id_message into outer0
                                   where x.Id_house == id_house
                                   from um0 in outer0.DefaultIfEmpty()
                                   orderby x.Created descending, x.Id
                                   select new { msg = x, usr = u, umsg = um0 }))
                {
                    var mv = new MessageV()
                    {
                        Id = (r.umsg != null && r.umsg.Id.HasValue) ? r.umsg.Id.Value : r.msg.Id,
                        Id_house = r.msg.Id_house,
                        Login = r.usr.Login,
                        Created = r.msg.Created.ToString("dd.MM.yyyy HH:mm:ss"),
                        Text = r.msg.Text,
                        Unread = (r.umsg != null)
                    };
                    result.Add(mv);
                }
                return result;
            }
        }

       
        public void Add(Message item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Message m = ctx.Messages.Add(item);
                    ctx.SaveChanges();
                    
                    foreach (var u in (from rh in ctx.RolesHouses
                                       join r in ctx.Roles on rh.Id_Role equals r.Id
                                       join u in ctx.Users on r.Id equals u.Id_Role
                                       where rh.Id_House == item.Id_house && u.Id != m.Id_user
                                       select new { Id_Message = m.Id, Id_User = u.Id }).Distinct())
                    {
                        var um = new UnreadMessage()
                        {
                            Id_message = u.Id_Message,
                            Id_user = u.Id_User
                        };
                        ctx.UnreadMessages.Add(um);
                    }
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }       
        

       /* public void Update(int id, Group item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Group g = (from x in ctx.Groups
                               where x.Id == id
                               select x).First();
                    g.Title = item.Title;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                     var f = new Group { Id = id };
                     ctx.Groups.Attach(f);
                     ctx.Groups.Remove(f);
                     ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }*/
    }
}
