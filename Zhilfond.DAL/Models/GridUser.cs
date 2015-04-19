using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [NotMapped]
    public class GridUser : User
    {
        public String RoleName { get; set; }
    }
}
