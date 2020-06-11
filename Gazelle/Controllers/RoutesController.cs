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
        public async Task<ActionResult<IEnumerable<int>>> Get(string origin, string destination, string weight)
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

            var resultEdges = new List<int>();
            foreach(var edge in path)
            {
                var graphNode = graph[edge] as Node<int, int>;
                resultEdges.Add(graphNode.Item);
            }

            return Ok(resultEdges);
        }

        public class RouteNode
        {
            public City City { get; set; }
            public int Index { get; set; }
        }
    }
}
