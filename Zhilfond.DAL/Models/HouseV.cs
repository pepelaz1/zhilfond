using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class HouseV
    {
        public int id { get; set; }
        public string nasp { get; set; }
        public string raion { get; set; }
        public string street { get; set; }
        public int? number { get; set; }
        public string letter { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public string oorg { get; set; }
        public string tdoma { get; set; }
        public string seriya { get; set; }
        public DateTime? date_exp { get; set; }
        public int? etaz_max { get; set; }
        public int? kolmunkv { get; set; }
        public int? ploo { get; set; }
        public int? kolzp { get; set; }
        public string fupr { get; set; }
    }
}
