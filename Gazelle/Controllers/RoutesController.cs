using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using Gazelle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly GazelleContext _context;

        public RoutesController(GazelleContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Route>> Get(string origin, string destination, string weight)
        {
            var graph = new Graph<int, int>();

            var connections = await _context.Connections.ToListAsync();
            var cities = await _context.Cities
                .OrderBy(x => x.CityId)
                .ToListAsync();

            var cityIds = cities.Select(x => x.CityId);

            foreach (var city in cityIds)
            {
                graph.AddNode(city);
            }

            foreach (var connection in connections)
            {
                graph.Connect((uint)connection.StartCity.CityId, (uint)connection.EndCity.CityId, connection.Price, 1);
            }

            var fromCityId = cities
                .First(x => x.CityName == origin)
                .CityId;
            var toCityId = cities
                .First(x => x.CityName == destination)
                .CityId;

            var result = graph.Dijkstra((uint)fromCityId, (uint)toCityId);
            var path = result.GetPath();

            if(path.Count() >= 2)
            {
                var resultConnections = new List<Connection>();

                for (int i = 1; i < path.Count(); i++)
                {
                    var firstElement = path.ElementAt(i - 1);
                    var secondElement = path.ElementAt(i);
                    var edgeConnection = connections
                        .Where(c => c.StartCity.CityId == firstElement && c.EndCity.CityId == secondElement)
                        .OrderBy(c => c.Price)
                        .First();
                    resultConnections.Add(edgeConnection);
                }

                var calcPrice = resultConnections.Sum(c => c.Price);
                var calcTime = resultConnections.Sum(c => c.Time);

                var route = new Route
                {
                    Price = calcPrice,
                    Time = calcTime,
                    Companies = "Telstar",
                    Connections = resultConnections
                };

                _context.Routes.Add(route);
                await _context.SaveChangesAsync();
                return route;
            } else
            {
                return NotFound();
            }
        }

        public class RouteNode
        {
            public City City { get; set; }
            public int Index { get; set; }
        }
    }
}
