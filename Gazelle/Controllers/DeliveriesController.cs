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
        public async Task<ActionResult<List<Delivery>>> Get()
        {
            List<Delivery> deliveryList = new List<Delivery>();

            deliveryList.Add(new Delivery
            {
                DeliveryId = 1,
                DriverId = 2,
                Weight = 35,
                DeliveryType = new DeliveryType
                {
                    DeliveryTypeId = 1,
                    Name = "frozen",
                    Price = 40
                },
                StartCity = _context.Cities.ToList().First(),
                EndCity = _context.Cities.ToList().Last(),
                Length = 40,
                ApprovedRoute = new Route
                {
                    RouteId = 1,
                    Price = 40,
                    Time = 50,
                    Companies = "Telstar",
                    Connections = new List<Connection>
                    {
                         new Connection
                        {
                            ConnectionId = 1,
                            Price = 40,
                            Time = 40,
                            Company = "Telstar",
                            StartCity = _context.Cities.ToList().First(),
                            EndCity = _context.Cities.ToList().Last()
                        }
                    }
                },
                Routes = new List<Route>
                {
                    new Route
                    {
                        RouteId = 1,
                        Price = 40,
                        Time = 50,
                        Companies = "Telstar",
                        Connections = new List<Connection>
                        {
                             new Connection
                            {
                                ConnectionId = 1,
                                Price = 40,
                                Time = 40,
                                Company = "Telstar",
                                StartCity = _context.Cities.ToList().First(),
                                EndCity = _context.Cities.ToList().Last()
                            }
                        }
                    }
                }
            });
            return Ok(deliveryList);
            //return await _context.Deliveries.ToListAsync();
        }

        [HttpPost]
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
