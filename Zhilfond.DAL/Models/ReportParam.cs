using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ReportParam
    {
        public int Id { get; set; }
        public int Id_group { get; set; }
        public int Id_form { get; set; }
        public int Id_field { get; set; }
        public int Id_category { get; set; }
        public bool Chosen { get; set; }
    }
}
