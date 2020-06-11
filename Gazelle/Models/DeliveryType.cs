using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Models
{
    public class DeliveryType
    {
        public int DeliveryTypeId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ICollection<Delivery> Deliveries { get; set;}
    }
}
