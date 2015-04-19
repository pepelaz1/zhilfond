using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int Id_house { get; set; }
        public string Text { get; set; }
        public int Id_user { get; set; }
        public DateTime Created { get; set; }
    }
}
