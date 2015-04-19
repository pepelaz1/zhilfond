using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int Id_user { get; set; }
        public DateTime Created { get; set; }
        public string House { get; set; }
        public string Element { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
        public string SourceType { get; set; }
        public int? Id_source { get; set; }
    }
}
