using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class RoleFormV
    {
        public int Id { get; set; }
        public int Id_Role { get; set; }
        public int Id_Form { get; set; }
        public int Id_Cat { get; set; }
        public int Id_Right { get; set; }
        public string Role { get; set; }
        public string Form { get; set; }
        public string Cat { get; set; }
        public string Right { get; set; }
    }
}
