using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    [Serializable]
    public class Session
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedTime { get; set; }
        public int UserId { get; set; }
    }
}
