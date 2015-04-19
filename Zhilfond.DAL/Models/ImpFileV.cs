using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ImpFileV
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Filename { get; set; }
        public DateTime Created { get; set; }
        public bool Signed { get; set; }
    }
}
