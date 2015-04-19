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
    public class UsersRepository : IUsersRepository
    {
        private String GetMD5(String text)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));
            return sb.ToString();
        }

        public IEnumerable<User> GetAll()
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    String query = @"SELECT u.Id, u.Login, u.UserName, u.Id_Role, r.Title, u.Active FROM UserContext.Users AS u LEFT OUTER JOIN UserContext.Roles AS r ON u.Id_Role = r.Id";                    
                    ObjectQuery<DbDataRecord> records = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DbDataRecord>(query);

                    List<GridUser> result = new List<GridUser>();

                    foreach (var r in records)
                        result.Add(new GridUser()
                        {
                            Id = Utils.ConvertFromDBVal<int>(r[0]),
                            Login = Utils.ConvertFromDBVal<string>(r[1]),
                            Username = Utils.ConvertFromDBVal<string>(r[2]),
                            Id_Role = Utils.ConvertFromDBVal<int>(r[3]),
                            RoleName = Utils.ConvertFromDBVal<string>(r[4]),
                            Active = Utils.ConvertFromDBVal<bool>(r[5])
                        });                    

                    return result;

                }
                catch (Exception ex)
                {
                    return null;
                }
            } 
        }

        public IEnumerable<GridUser> GetAll(String sfld, String sord, String srfield, String srop, String srvalue)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    String query = @"SELECT u.Id, u.Login, u.UserName, u.Id_Role, r.Title, u.Active
                                             FROM UserContext.Users AS u LEFT OUTER JOIN UserContext.Roles AS r ON u.Id_Role = r.Id";

                    if (!String.IsNullOrEmpty(srfield))
                        query += " WHERE p." + srfield + (srop == "eq" ? "=" : "<>") + srvalue;


                    if (!String.IsNullOrEmpty(sfld))
                        query += " ORDER BY p." + sfld + " " + sord;

                    ObjectQuery<DbDataRecord> records = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DbDataRecord>(query);
                    

                    //ObjectQuery<DbDataRecord> queryX = new ObjectQuery<DbDataRecord>("SELECT u.Id, u.Login, u.UserName, u.Id_Role, r.Title, u.Active"+
                    //                         "FROM UserContext.Users AS u LEFT OUTER JOIN UserContext.Roles AS r ON u.Id_Role = r.Id",
                    //                         new System.Data.Objects.ObjectContext("Server=tst105-w;Database=zhilfond;User Id=postgres;Password=`1qwerty;"));

                    List<GridUser> result = new List<GridUser>();

                    foreach (var r in records)
                        result.Add(new GridUser()
                        {
                            Id = Utils.ConvertFromDBVal<int>(r[0]),
                            Login = Utils.ConvertFromDBVal<string>(r[1]),
                            Username = Utils.ConvertFromDBVal<string>(r[2]),
                            Id_Role = Utils.ConvertFromDBVal<int>(r[3]),
                            RoleName = Utils.ConvertFromDBVal<string>(r[4]),
                            Active = Utils.ConvertFromDBVal<bool>(r[5])
                        });

                    //foreach (var r in records)
                    //{
                    //    GridUser gu = new GridUser();

                    //    gu.Id = Utils.ConvertFromDBVal<int>(r[0]);
                    //    gu.Login = Utils.ConvertFromDBVal<string>(r[1]);
                    //    gu.Username = Utils.ConvertFromDBVal<string>(r[2]);
                    //    gu.Id_Role = Utils.ConvertFromDBVal<int>(r[3]);
                    //    gu.RoleName = Utils.ConvertFromDBVal<string>(r[4]);
                    //    gu.Active = Utils.ConvertFromDBVal<bool>(r[5]);
                    //    //gu.Active = Boolean.Parse(r[5].ToString());

                    //    result.Add(gu);
                    //}

                    return result;

                }
                catch (Exception ex)
                {
                    return null;
                }
            }            
        }

        //        public IEnumerable<User> GetAll(String sfld, String sord, String srfield, String srop, String srvalue)
        //        {
        //            using (var ctx = new UserContext())
        //            {
        //                try
        //                {
        //                    String query = @"SELECT u.Id, u.Login, u.UserName, u.Id_Role, r.Title, u.Active
        //                                             FROM UserContext.Users AS u JOIN UserContext.Roles AS r ON u.Id_Role = r.Id";

        //                    if (!String.IsNullOrEmpty(srfield))
        //                        query += " WHERE p." + srfield + (srop == "eq" ? "=" : "<>") + srvalue;


        //                    if (!String.IsNullOrEmpty(sfld))
        //                        query += " ORDER BY p." + sfld + " " + sord;

        //                    ObjectQuery<DbDataRecord> records = ((IObjectContextAdapter)ctx).ObjectContext.CreateQuery<DbDataRecord>(query);
        //                    List<User> result = new List<User>();

        //                    foreach (var r in records)
        //                        result.Add(new User()
        //                        {
        //                            Id = Int32.Parse(r[0].ToString()),
        //                            Login = r[1].ToString(),
        //                            Username = r[2].ToString(),
        //                            Id_Role = Int32.Parse(r[3].ToString()),
        //                            RoleName = r[4].ToString(),
        //                            Active = Boolean.Parse(r[5].ToString())
        //                        });

        //                    return result;

        //                }
        //                catch (Exception ex)
        //                {
        //                    return null;
        //                }
        //            }
        //        }

        public User Get(int id)
        {
            using (var ctx = new UserContext())
            {
                return ctx.Users.Single(p => p.Id == id);
            }
        }

        public User Get(String login, String passwordHash)
        {
            using (var ctx = new UserContext())
            {
                return ctx.Users.FirstOrDefault((p => p.Login == login && p.Password.ToLower() == passwordHash.ToLower()));
            }
        }

        public User GetByToken(string token)
        {
            using (var ctx = new UserContext())
            {
                return (from u in ctx.Users
                        join s in ctx.Sessions on u.Id equals s.UserId
                        where s.Token == token
                        select u).FirstOrDefault();
            }
        }

        public User GetByLogin(string login)
        {
            using (var ctx = new UserContext())
            {
                return (from u in ctx.Users
                        where u.Login == login
                        select u).FirstOrDefault();
            }
        }

        public UserV GetVByToken(string token)
        {
            using (var ctx = new UserContext())
            {
                return (from u in ctx.Users
                        join s in ctx.Sessions on u.Id equals s.UserId
                        join r in ctx.Roles on u.Id_Role equals r.Id
                        where s.Token == token
                        select new UserV { Id = u.Id, Login = u.Login, Username = u.Username, Id_Role = r.Id, Role = r.Title, Active = u.Active }).FirstOrDefault();
            }
        }

        public User Add(User item)
        {
            using (var ctx = new UserContext())
            {
                // Set user password same as username
                String passwordHash = GetMD5(item.Username);

                item = ctx.Users.Add(new User()
                {
                    Username = item.Username,
                    Login = item.Login,
                    Password = passwordHash,
                    Id_Role = item.Id_Role,
                    Active = item.Active
                });

                //// write audit record
                //ctx.Audits.Add(new Audit()
                //{
                //    OperationType = "insert",
                //    OperationTable = "UserAccounts",
                //    OldRecord = "",
                //    NewRecord = item.Username + "|" + item.Login + "|" + item.Id_Role + "|" + item.Active.ToString()
                //});

                ctx.SaveChanges();
            }

            return item;
        }

        public void Delete(int id)
        {
            using (var ctx = new UserContext())
            {
                var user = ctx.Users.Single(p => p.Id == id);

                //// write audit record
                //ctx.Audits.Add(new Audit()
                //{
                //    OperationType = "delete",
                //    OperationTable = "UserAccounts",
                //    OldRecord = user.Username + "|" + user.Id_Role + "|" + user.Mobile + "|" + user.Country + "|" + user.Active.ToString() + "|" + user.UpdatedDate.ToShortDateString() + "|" + user.UpdatedFrom + "|" + user.UpdatedBy,
                //    NewRecord = "",
                //    UpdatedBy = UpdatedBy,
                //    UpdatedTime = DateTime.Now,
                //    UpdatedFrom = UpdatedFrom
                //});

                ctx.Users.Remove(user);
                ctx.SaveChanges();
            }
        }

        public bool Update(int id, User item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    var user = ctx.Users.Single(p => p.Id == id);

                    //// write audit record
                    //ctx.Audits.Add(new Audit()
                    //{
                    //    OperationType = "update",
                    //    OperationTable = "UserAccounts",
                    //    OldRecord = user.Username + "|" + user.Id_Role + "|" + user.Mobile + "|" + user.Country + "|" + user.Active.ToString() + "|" + user.UpdatedDate.ToShortDateString() + "|" + user.UpdatedFrom + "|" + user.UpdatedBy,
                    //    NewRecord = item.Username + "|" + item.Id_Role + "|" + item.Mobile + "|" + item.Country + "|" + item.Active.ToString() + "|" + item.UpdatedDate.ToShortDateString() + "|" + item.UpdatedFrom + "|" + item.UpdatedBy,
                    //    UpdatedBy = item.UpdatedBy,
                    //    UpdatedTime = DateTime.Now,
                    //    UpdatedFrom = item.UpdatedFrom
                    //});

                    user.Username = item.Username;
                    user.Login = item.Login;
                    user.Id_Role = item.Id_Role;
                    user.Active = item.Active;

                    ctx.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool ChangePassword(ChPwd chpwd)
        {
            using (var ctx = new UserContext())
            {
                String oldpassHash = GetMD5(chpwd.OldPass);
                String newpassHash = GetMD5(chpwd.NewPass);
                String repeat = GetMD5(chpwd.Repeat);

                var user = ctx.Users.FirstOrDefault(p => p.Id == chpwd.UserId && p.Password.ToLower() == oldpassHash.ToLower());
                if (user == null)
                    return false;

                user.Password = newpassHash;
                ctx.SaveChanges();
            }
            return true;
        }


        

    }
}
