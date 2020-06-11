using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Models
{
    public class Connection
    {
        public int ConnectionId { get; set; }
        public int Price { get; set; }
        public int Time { get; set; }
        public string Company { get; set; }
        public City StartCity { get; set; }
        public City EndCity { get; set; }
    }
}
