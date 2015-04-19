using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ZValueAll
    {
        public int Id { get; set; }
        public int Id_house { get; set; } // 382
        public int Id_form { get; set; }
        public int Id_field { get; set; }
        public string Value { get; set; }
        public int? Id_parent { get; set; }
        public string Warning { get; set; }
    }
}
