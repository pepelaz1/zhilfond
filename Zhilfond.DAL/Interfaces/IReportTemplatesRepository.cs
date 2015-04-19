using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IReportTemplatesRepository
    {
        IEnumerable<ReportTemplate> Get();
        void Add(ReportTemplate item);
        ReportTemplate GetById(int id);
        ReportTemplate GetByName(string sTemplateName);
        bool Update(int id, ReportTemplate item);
        void Delete(int id);
    }
}
