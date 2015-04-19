using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class GroupHouseV
    {
        public int Id { get; set; }
        public int Id_group { get; set; }
        public string Street { get; set; }
        public int? Number { get; set; }
        public string Letter { get; set; }
        public int Id_house { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string Baloon { get; set; }
    }
}
