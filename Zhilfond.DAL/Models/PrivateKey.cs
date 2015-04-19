using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class PrivateKey
    {
        public int Id { get; set; }
        public int Id_user { get; set; }
        public string KeyValue { get; set; }
    }
}
