using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ImpFile
    {
        public int Id { get; set; }
        public int Id_user { get; set; }
        public string Filename { get; set; }
        public DateTime Created { get; set; }
        public bool Signed { get; set; }
    }
}
