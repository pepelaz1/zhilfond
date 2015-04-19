using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class AuditV
    {
        public int Id { get; set; }
        public int Id_house { get; set; }
        public string Login { get; set; }
        public string form { get; set; }
        public string field { get; set; }
        public string OldVal { get; set; }
        public string NewVal { get; set; }
        public DateTime whendatetime { get; set; }
    }
}
