using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IReportValuesRepository
    {
        IEnumerable<ReportValue> Get(int id_group, int id_template);
    }
}
