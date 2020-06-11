using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gazelle.Models;
using System;

namespace Gazelle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionsController : ControllerBase
    {
        private readonly GazelleContext _context;

        private readonly string[] _deliveryTypes = new string[] { "frozen", "fragile", "weapon", "recordedDelivery", "animal" };
        private readonly string[] _supportedDeliveryTypes = new string[] { "frozen", "fragile", "recordedDelivery", "animal" };

        public ConnectionsController(GazelleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Connection>> Get(string origin, string destination, int weight, int length, int height, int depth, string deliveryTypes)
        {
            if((weight > 40 && weight > 0) || string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination))
            {
                return NotFound();
            }

            if(deliveryTypes != null)
            {
                var deliveryTypesArray = deliveryTypes.Split(',');

                foreach (var deliveryType in deliveryTypesArray)
                {
                    var substrings = deliveryType.Split("=");
                    var key = substrings[0];
                    var value = bool.Parse(substrings[1]);

                    var knownDeliveryType = _deliveryTypes.FirstOrDefault(x => x == key);
                    if (knownDeliveryType == null)
                    {
                        return NotFound();
                    }

                    if (value && !_supportedDeliveryTypes.Contains(knownDeliveryType))
                    {
                        return NotFound();
                    }
                }
            }

            var fromCityInvariant = origin.ToLower();
            var toCityInvariant = destination.ToLower();

            var connection = await _context.Connections
                .Include(x => x.StartCity)
                .Include(x => x.EndCity)
                .FirstOrDefaultAsync(x =>
                x.StartCity.CityName.ToLower() == fromCityInvariant &&
                x.EndCity.CityName.ToLower() == toCityInvariant);

            if (connection == null)
            {
                return NotFound();
            }

            return connection;
        }
    }
}
