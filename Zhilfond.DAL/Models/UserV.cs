using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace DAL.Models
{
    public class UserV
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public int Id_Role { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
    }
}