using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public int Id_operation { get; set; }
        public int Id_field { get; set; }
        public string Title { get; set; }
        public string Predicate { get; set; }
    }
}
