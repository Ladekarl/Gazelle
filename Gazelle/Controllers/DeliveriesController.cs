using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gazelle.Models;

namespace Gazelle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly GazelleContext _context;

        public class CreateDeliveryModel
        {
            public string OriginName { get; set; }
            public string DestinationName { get; set; }
            public int DeliveryType { get; set; }
            public double Size { get; set; }
            public Route ApprovedRoute { get; set; }
        }

        public class ApproveDeliveryModel
        {
            public int DeliveryId { get; set; }
            public int ApprovedRouteId { get; set; }
        }

        public DeliveriesController(GazelleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliveries()
        {
            return await _context.Deliveries.ToListAsync();
        }

        [HttpGet(Name = "Routes")]
        public async Task<ActionResult<Route>> GetRoutes(string origin, string destination, string weight)
        {
            var routes = new List<Route>();

            var connections = new List<Connection>();
            connections.Add(new Connection
            {
                ConnectionId = 1,
                Price = 50,
                Time = 500,
                Company = "Telstar",
                StartCity = new City { CityId = Guid.NewGuid(), CityName = "Dakar", Country = new Country { CountryId = 1, Conflict = false, Name = "Marokko" } },
                EndCity = new City
                {
                    CityId = Guid.NewGuid(),
                    CityName = "Marakkesh",
                    Country = new Country { CountryId = 1, Conflict = false, Name = "Marokko" },
                }
            });

            routes.Add(new Route
            {
                RouteId = 1,
                Price = 50,
                Time = 500,
                Companies = "Telstar,EastIndia",
                Connections = connections
            });

            return Ok(routes);
        }

        [HttpPost(Name = "Approve")]
        public async Task<ActionResult<Delivery>> ApproveDelivery([FromBody] ApproveDeliveryModel approveDeliveryModel)
        {
            var delivery = await _context.Deliveries
                .Include(x => x.Routes)
                .FirstOrDefaultAsync(x => x.DeliveryId == approveDeliveryModel.DeliveryId);

            if (delivery == null)
            {
                return NotFound();
            }

            var approvedRoute = delivery.Routes.FirstOrDefault(x => x.RouteId == approveDeliveryModel.ApprovedRouteId);

            if (approvedRoute == null)
            {
                return NotFound();
            }

            delivery.ApprovedRoute = approvedRoute;

            await _context.SaveChangesAsync();

            return Ok(delivery);
        }
    }
}
