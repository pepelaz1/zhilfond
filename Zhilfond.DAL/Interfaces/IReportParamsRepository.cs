using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IReportParamsRepository
    {
        IEnumerable<ReportParamV> GetAllV(int id_group);
        IEnumerable<ReportParam> GetAll(int id_group);
        void Put(ReportParam rp);
    }
}
