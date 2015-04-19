using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class House
    {
        public int Id { get; set; }
        public int? id_location { get; set; }
        public int? id_raion { get; set; }
        public int? id_street { get; set; }
        public int? number { get; set; }
        public string letter { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public int? id_oorg { get; set; }
        public int? id_tdoma { get; set; }
        public string seriya { get; set; }
        public DateTime? date_exp { get; set; }
        public int? etaz_max { get; set; }
        public int? kolmunkv { get; set; }
        public int? ploo { get; set; }
        public int? kolzp { get; set; }
        public int? id_fupr { get; set; }
    }
}
