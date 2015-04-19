using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IHousesRepository
    {
        IEnumerable<HouseV> GetAllView(IEnumerable<KeyValuePair<string,string>> map);
        IEnumerable<House> GetAllIds();
        int GetPageNo(int id_house, int pagesize);
    }
}
