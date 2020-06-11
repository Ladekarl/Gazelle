using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Models
{
    public class GazelleContext: DbContext
    {
        public GazelleContext(DbContextOptions<GazelleContext> options) : base(options)
        { }

        public DbSet<City> Cities { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }
        public DbSet<Route> Routes { get; set; }

    }
}
