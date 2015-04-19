using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace DAL.Models
{
    public class UnreadMessage
    {
        public int? Id { get; set; }
        public int? Id_user { get; set; }
        public int? Id_message { get; set; }
    }
}