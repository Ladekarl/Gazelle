using Gazelle.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoutesController: ControllerBase
    {
        private readonly GazelleContext _context;

        public RoutesController(GazelleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Route>> Get(string origin, string destination, string weight)
        {
            var routes = new List<Route>();

            var connections = new List<Connection>();
            connections.Add(new Connection
            {
                ConnectionId = 1,
                Price = 50,
                Time = 500,
                Company = "Telstar",
                StartCity = new City { CityId = 1, CityName = "Dakar", Country = new Country { CountryId = 1, Conflict = false, Name = "Marokko" } },
                EndCity = new City
                {
                    CityId = 1,
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
    }
}
