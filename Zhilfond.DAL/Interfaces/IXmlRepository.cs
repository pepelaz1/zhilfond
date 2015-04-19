using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IXmlRepository
    {
        string Generate(int id_group);
    }
}
