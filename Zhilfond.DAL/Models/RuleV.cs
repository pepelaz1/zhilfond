using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class RuleV
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public int Id_field { get; set; }
        public string Title { get; set; }
        public string Predicate { get; set; }
    }
}
