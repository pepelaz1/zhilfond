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
    public class GenReportsRepository : IGenReportsRepository
    {
        static readonly IPrivateKeysRepository _priv_repository = new PrivateKeysRepository();

        public IEnumerable<GenReport> Get(int id_user)
        {
            using (var ctx = new UserContext())
            {
                List<GenReport> result = new List<GenReport>();
                foreach (var r in (from x in ctx.GenReports
                                   where x.Id_user == id_user
                                   orderby x.Created descending
                                   select x/*new GenReport { Id = x.Id, Filename = x.Filename, Created = x.Created, Type = x.Type, Signature = x.Signature}*/))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public GenReport GetById(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    return (from x in ctx.GenReports
                            where x.Id == id
                            select x).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IEnumerable<GenReport> GetByRoleId(int role_id)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    List<GenReport> result = new List<GenReport>();

                    foreach (var rec in (from x in ctx.GenReports
                                         join u in ctx.Users on x.Id_user equals u.Id
                                         join r in ctx.Roles on u.Id_Role equals r.Id
                                         where r.Id == role_id
                                         select x).OrderByDescending(x => x.Created))
                    {
                        result.Add(rec);
                    }

                    //result.Reverse();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public GenReport GetByContent(byte[] content)
        {
            using (var ctx = new UserContext())
            {
                try
                {

                    return (from x in ctx.GenReports
                            //where x.Data.SequenceEqual(content)
                            where x.Data == content
                            select x).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public GenReport Add(GenReport item)
        {
            using (var ctx = new UserContext())
            {
                // Подписываем данные при помощи приватного ключа пользователя admin
                PrivateKey key = _priv_repository.Get();
                var sc = new SignatureCreator();
                item.Signature = sc.Sign(item.Data, key.KeyValue);


                item = ctx.GenReports.Add(item);
                ctx.SaveChanges();
            }
            return item;
        }

        public bool Validate(byte[] content)
        {
            PrivateKey key = _priv_repository.Get();
            var sc = new SignatureCreator();
            string signature = sc.Sign(content, key.KeyValue);

            var target = GetByContent(content);

            if (signature == target.Signature)
                return true;

            return false;
        }


    }
}
