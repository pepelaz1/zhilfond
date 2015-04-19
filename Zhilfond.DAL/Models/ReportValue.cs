using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ReportValue
    {
        public int Id_house { get; set; }
        public int Id_form { get; set; }
        public string Form { get; set; }
        public int Id_field { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public int? Id_parent { get; set; }
    }
}
