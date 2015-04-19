using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface ICoordsRepository
    {
        bool Update(Coords coords);
        Coords Get(int id_house);
    }
}
