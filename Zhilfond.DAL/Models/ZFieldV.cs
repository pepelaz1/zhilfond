using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ZFieldV
    {
        public int Id { get; set; }
        public int Id_form { get; set; }
        public string Title { get; set; }
        public string Typename { get; set; }
        public string Category { get; set; }
        public int? Order { get; set; }
        public bool? Required { get; set; }
        public bool? Personal { get; set; }
    }
}
