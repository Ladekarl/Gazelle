using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public double DriverId { get; set; }
        public double Weight { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public City StartCity { get; set; }
        public City EndCity { get; set; }
        public double Length { get; set; }
        public Route ApprovedRoute { get; set; }
        public ICollection<Route> Routes { get; set; }

    }
}
