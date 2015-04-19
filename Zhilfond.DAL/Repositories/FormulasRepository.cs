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
    public class FormulasRepository : IFormulasRepository
    {
        public IEnumerable<Formula> Get(int id_field)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    List<Formula> result = new List<Formula>();
                    foreach (var f in (from f in ctx.Formulas
                                       where f.Id_field == id_field
                                       orderby f.Title
                                       select f))
                    {
                        result.Add(f);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new List<Formula>();
            }
        }

        public void Add(Formula item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Formula f = ctx.Formulas.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Update(int id, Formula item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Formula f = (from x in ctx.Formulas
                                 where x.Id == id
                                 select x).First();
                    f.Title = item.Title;
                    f.Predicate = item.Predicate;
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
                    var r = new Formula { Id = id };
                    ctx.Formulas.Attach(r);
                    ctx.Formulas.Remove(r);
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
