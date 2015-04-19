using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace DAL.Models
{
    [Serializable]
    public class GenReport
    {
        public int Id { get; set; }
        public int Id_user { get; set; }
        public string Filename { get; set; }
        public byte[] Data { get; set; }
        public DateTime Created { get; set; }
        public string Type { get; set; }
        public string Signature { get; set; }
    }
}