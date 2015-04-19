using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace DAL.Models
{
    public class UnreadMessageV
    {
        public int Id { get; set; }
        public int Id_house { get; set; }
        public string Login { get; set; }
        public DateTime WhenDateTime { get; set; }
        public string Text { get; set; }
    }
}