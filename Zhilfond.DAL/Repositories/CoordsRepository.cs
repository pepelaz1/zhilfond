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
    public class CoordsRepository : ICoordsRepository
    {
        public bool Update(Coords coords)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        House h = (from x in ctx.Houses
                                   where x.Id == coords.Id_house
                                   select x).First();
                        h.latitude = coords.Latitude;
                        h.longitude = coords.Longitude;
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Coords Get(int id_house)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    return (from x in ctx.Houses
                            where x.Id == id_house
                            select new Coords { Id_house = x.Id, Latitude = x.latitude, Longitude = x.longitude }).First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
