using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public int Id_house { get; set; }
        public string Filename { get; set; }
        public byte[] Data { get; set; }
        public int? Order { get; set; }
        public bool Archive { get; set; }
    }
}
