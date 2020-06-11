using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public int Price { get; set; }
        public int Time { get; set; }
        public string Companies { get; set; }
        public ICollection<Connection> Connections { get; set; }
    }
}
