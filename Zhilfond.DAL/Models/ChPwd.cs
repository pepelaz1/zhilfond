using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class ChPwd
    {
        public int UserId { get; set; }
        public String OldPass { get; set; }
        public String NewPass { get; set; }
        public String Repeat { get; set; }
    }
}
