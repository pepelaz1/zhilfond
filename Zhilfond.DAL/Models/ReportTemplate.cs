using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ReportTemplate
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Reportname { get; set; }
        public byte[] Data { get; set; }
        public int Reporttype { get; set; }
    }
}
