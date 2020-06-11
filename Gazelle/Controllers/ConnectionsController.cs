using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gazelle.Models;

namespace Gazelle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionsController : ControllerBase
    {
        private readonly GazelleContext _context;

        public ConnectionsController(GazelleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Connection>> Get(string origin, string destination, int weight, int length, int height, int depth, bool frozen, bool fragile, bool weapon, bool recordedDelivery, bool animal, bool recommendedDelivery)
        {
            if(weight > 40 || weapon)
            {
                return NotFound();
            }

            var fromCityInvariant = origin.ToLower();
            var toCityInvariant = destination.ToLower();

            var connection = await _context.Connections
                .Include(x => x.StartCity)
                .Include(x => x.EndCity)
                .FirstOrDefaultAsync(x =>
                x.StartCity.CityName == fromCityInvariant &&
                x.EndCity.CityName == toCityInvariant);

            if (connection == null)
            {
                return Ok();
            }

            return connection;
        }
    }
}
