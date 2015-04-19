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
    public class ImpFilesRepository : IImpFilesRepository
    {
        public IEnumerable<ImpFileV> Get(int id_user)
        {
            using (var ctx = new UserContext())
            {
                var user = ctx.Users.Where(x => x.Id == id_user).FirstOrDefault();

                List<ImpFileV> result = new List<ImpFileV>();
                foreach (var r in (from x in ctx.ImpFiles
                                   join u in ctx.Users on x.Id_user equals u.Id
                                   join r in ctx.Roles on u.Id_Role equals r.Id
                                   where r.Id == user.Id_Role
                                   && x.Signed == true
                                   orderby x.Created descending
                                   select new ImpFileV { Id = x.Id, Filename = x.Filename, Created = x.Created, Username = u.Login }))
                {
                    result.Add(r);
                }
                return result;
            }
        }

       
        public ImpFile Add(ImpFile item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    var imf = ctx.ImpFiles.Add(item);
                    ctx.SaveChanges();
                    return imf;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void SignFile(int fileId) {
            using (var ctx = new UserContext())
            {
                try
                {
                    var imf = (from x in ctx.ImpFiles
                               where x.Id == fileId
                               select x).FirstOrDefault();
                    imf.Signed = true;
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
