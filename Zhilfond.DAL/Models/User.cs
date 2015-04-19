using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace DAL.Models
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public String Login { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public int Id_Role { get; set; }        
        public Boolean Active { get; set; }
    }
}