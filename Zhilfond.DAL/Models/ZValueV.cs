using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ZValueV
    {
        public int Id { get; set; }
        public int Id_house { get; set; } // 382
        public int Id_form { get; set; }
        public int Id_field { get; set; }
        public string Value { get; set; }

        public int Id_dict { get; set; } // Dictionary identifier
        public string Dictname { get; set; } // Dictionary name ( including primitive like 'string', 'integer')

        public string Title { get; set; } // Field title
        public int Id_category { get; set; } 
        public string Category { get; set; } // Field category

        public int? Id_parent { get; set; } // 

        public string Warning { get; set;}
    }
}
