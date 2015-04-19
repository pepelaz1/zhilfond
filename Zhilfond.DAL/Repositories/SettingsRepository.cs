using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;


namespace DAL.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        public void Update(Settings settings)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    var col = (from x in ctx.Settings
                               select x);
                    if (col.Count() > 0)
                    {
                        var s = col.First();
                        s.BaloonTemplate = settings.BaloonTemplate;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.Settings.Add(settings);
                        ctx.SaveChanges();
                     }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Settings Get()
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    return (from x in ctx.Settings
                            select x).First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
