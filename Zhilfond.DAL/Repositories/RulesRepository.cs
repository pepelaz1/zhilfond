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
    public class RulesRepository : IRulesRepository
    {
        public IEnumerable<RuleV> Get(int id_field)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    List<RuleV> result = new List<RuleV>();
                    foreach (var r in (from r in ctx.Rules
                                       join o in ctx.RuleOperations on r.Id_operation equals o.Id
                                       where r.Id_field == id_field
                                       orderby r.Title
                                       select new RuleV { Id = r.Id, Title = r.Title, Operation = o.Operation, Predicate = r.Predicate }))
                    {
                        result.Add(r);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new List<RuleV>();
            }
        }

        public void Add(Rule item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Rule r = ctx.Rules.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Update(int id, Rule item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Rule r = (from x in ctx.Rules
                              where x.Id == id
                              select x).First();
                    r.Title = item.Title;
                    r.Predicate = item.Predicate;
                    r.Id_operation = item.Id_operation;
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
                    var r = new Rule { Id = id };
                    ctx.Rules.Attach(r);
                    ctx.Rules.Remove(r);
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
