using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IValuesRepository
    {
        IEnumerable<ZValueV> Get(int id_form, int id_house, int? id_parent, UserV u);
        IEnumerable<ZValueV> Get(string form, int id_house, int? id_parent, UserV u);
        IEnumerable<dynamic> GetDisplayNames(string form, int id_house);
        IEnumerable<ZFieldV> GetDetails(int id_form);
        void Process(int id_user, string sourceType, string id_source, string xml, string signature, string cert);        
    }
}
