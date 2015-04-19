using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ReportParamV
    {
        public int? Id { get; set; }
        public int Id_form {get;set;}
        public string Form { get; set; }
        public int Id_field {get;set;}
        public string Field { get; set; }
        public int? Id_category { get; set; }
        public string Category { get; set; }
        public bool? Chosen { get; set; }
    }
}
