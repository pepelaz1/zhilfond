using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public int Id_house { get; set; }
        public int Id_user { get; set; }
        public int Id_form { get; set; }
        public int Id_field { get; set; }
        public string OldVal { get; set; }
        public string NewVal { get; set; }
        public DateTime WhenDateTime { get; set; }
    }
}
