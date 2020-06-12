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
            public double DriverId { get; set; }
            public double Weight { get; set; }
            public string StartCity { get; set; }
            public string EndCity { get; set; }
            public int ApprovedRouteId { get; set; }
        }

        public DeliveriesController(GazelleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> Get()
        {
            return await _context.Deliveries
                .Include(d => d.StartCity)
                .Include(d => d.EndCity)
                .Include(d => d.ApprovedRoute)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Delivery>> Post([FromBody] ApproveDeliveryModel approveDeliveryModel)
        {
            var delivery = new Delivery
            {
                DriverId = approveDeliveryModel.DriverId,
                Weight = approveDeliveryModel.Weight,
                StartCity = _context.Cities.First(c => c.CityName == approveDeliveryModel.StartCity),
                EndCity = _context.Cities.First(c => c.CityName == approveDeliveryModel.EndCity),
                ApprovedRoute = _context.Routes.First(a => a.RouteId == approveDeliveryModel.ApprovedRouteId)
            };

            _context.Deliveries.Add(delivery);

            await _context.SaveChangesAsync();

            return Ok(delivery);
        }
    }
}
