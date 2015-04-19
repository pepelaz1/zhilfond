using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class MessageV
    {
        public int Id { get; set; }
        public int Id_house { get; set; }
        public string Text { get; set; }
        public string Login { get; set; }
        public string Created { get; set; }
        public bool Unread { get; set; }
    }
}
