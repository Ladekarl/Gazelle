using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gazelle.Models;
using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;

namespace Gazelle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionsController : ControllerBase
    {
        private readonly GazelleContext _context;

        private readonly string[] _deliveryTypes = new string[] { "frozen", "fragile", "weapon", "recordedDelivery", "animal", "war" };
        private readonly string[] _supportedDeliveryTypes = new string[] { "frozen", "fragile", "recordedDelivery", "animal", "war" };

        public class ConnectionDto
        {
            public int? Price { get; set; }
            public int? Time { get; set; }
        }

        public ConnectionsController(GazelleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ConnectionDto>> Get(string origin, string destination, int weight, int length, int height, int depth, string deliveryTypes)
        {
            if(weight > 40 || weight < 0 || string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination))
            {
                return NotFound();
            }

            var sentTypes = new List<DeliveryType>();
            if(deliveryTypes != null)
            {
                var deliveryTypesArray = deliveryTypes.Split(',');

                foreach (var deliveryType in deliveryTypesArray)
                {
                    var knownDeliveryType = _deliveryTypes.FirstOrDefault(x => x == deliveryType);
                    if (knownDeliveryType == null)
                    {
                        return NotFound();
                    }

                    if (!_supportedDeliveryTypes.Contains(knownDeliveryType))
                    {
                        return NotFound();
                    }

                    var dbType = _context.DeliveryTypes.First(x => x.Name == knownDeliveryType);
                    sentTypes.Add(dbType);
                }
            }

            var fromCityInvariant = origin.ToLower();
            var toCityInvariant = destination.ToLower();

            var connection = await _context.Connections
                .Include(x => x.StartCity)
                .Include(x => x.EndCity)
                .FirstOrDefaultAsync(x =>
                x.StartCity.CityName.ToLower() == fromCityInvariant &&
                x.EndCity.CityName.ToLower() == toCityInvariant &&
                x.Company == "Telstar");

            if (connection == null)
            {
                return NotFound();
            }

            var totalPriceAddition = sentTypes.Sum(x => x.Price);

            var conn = new ConnectionDto
            {
                Price = connection.Price.Value + (connection.Price.Value * (totalPriceAddition / 100)),
                Time = connection.Time.Value
            };

            return Ok(conn);
        }
    }
}
