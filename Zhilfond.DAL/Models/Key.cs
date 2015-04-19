using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class Key
    {
        public int Id { get; set; }
        public int Id_User { get; set; }
        public string KeyValue { get; set; }
        public DateTime Date_Start { get; set; }
        public DateTime Date_Finish { get; set; }
    }
}
