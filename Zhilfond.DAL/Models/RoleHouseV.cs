using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class RoleHouseV
    {
        public int Id { get; set; }
        public int Id_Role { get; set; }
        public int Id_House { get; set; }
        public int Id_Right { get; set; }
        public string Role { get; set; }
        public string Right { get; set; }
    }
}
