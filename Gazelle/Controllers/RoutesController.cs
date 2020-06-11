using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using Gazelle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Gazelle.Controllers.ConnectionsController;

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
        public async Task<ActionResult<IEnumerable<Route>>> Get(string origin, string destination, int weight, int length, int height, int depth, string deliveryTypes)
        {
            if (weight > 40)
            {
                return NotFound();
            } 

            var cheapestRoute = await CalculateCheapestRoute(origin, destination, c =>
            {
                var prices = new List<int>();
                var eastIndia = GetFromRemote("http://wa-eitdk.azurewebsites.net/api/getshippinginfo", origin, destination, weight, length, height, depth, deliveryTypes);
                var oceanic = GetFromRemote("http://wa-oadk.azurewebsites.net/api/routeApi", origin, destination, weight, length, height, depth, deliveryTypes);

                if (eastIndia != null)
                {
                    prices.Add(eastIndia.Price);
                }

                if (oceanic != null)
                {
                    prices.Add(oceanic.Price);
                }
                prices.Add(c.Price);

                return prices.Min();
            });

            var shortestRoute = await CalculateShortestRoute(origin, destination, c =>
            {
                var times = new List<int>();
                var eastIndia = GetFromRemote("http://wa-eitdk.azurewebsites.net/api/getshippinginfo", origin, destination, weight, length, height, depth, deliveryTypes);
                var oceanic = GetFromRemote("http://wa-oadk.azurewebsites.net/api/routeApi", origin, destination, weight, length, height, depth, deliveryTypes);

                if(eastIndia != null)
                {
                    times.Add(eastIndia.Time);
                }
                
                if(oceanic != null)
                {
                    times.Add(oceanic.Time);
                }

                times.Add(c.Time);

                return times.Min();
            });

            if (cheapestRoute == null || shortestRoute == null)
            {
                return NotFound();
            }

            return new List<Route> { cheapestRoute, shortestRoute };
        }

        private async Task<Route> CalculateCheapestRoute(string origin, string destination, Func<ConnectionDto, int> comparison)
        {
            var connections = await _context.Connections.ToListAsync();
            var path = await CalculateRoute(origin, destination, connections, comparison);

            if (path.Count() >= 2)
            {
                return await GetRouteFromPath(path, connections);
            }

            return null;
        }

        private async Task<Route> GetRouteFromPath(IEnumerable<uint> path, ICollection<Connection> connections)
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
        }

        private async Task<Route> CalculateShortestRoute(string origin, string destination, Func<ConnectionDto, int> comparison)
        {
            var connections = await _context.Connections.ToListAsync();
            var path = await CalculateRoute(origin, destination, connections, comparison);

            if (path.Count() >= 2)
            {
                return await GetRouteFromPath(path, connections);
            }

            return null;
        }

        private ConnectionDto GetFromRemote(string url, string origin, string destination, int weight, int length, int height, int depth, string deliveryTypes)
        {
            WebRequest request = WebRequest.Create(string.Format("{0}?origin={1}&destination={2}&weight={3}&length={4}&height={5}&depth={6}&deliveryTypes={7}", url, origin, destination, weight, length, height, depth, deliveryTypes));
            request.Method = "GET";

            HttpWebResponse response = ((HttpWebRequest) request).GetResponseNoException();

            if (response != null && response.StatusDescription == "OK")
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ConnectionDto>(responseFromServer);
            }
            return null;
        }

        private async Task<IEnumerable<uint>> CalculateRoute(string origin, string destination, IEnumerable<Connection> connections, Func<ConnectionDto, int> comparison)
        {
            var graph = new Graph<int, int>();

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
                var connectionDto = new ConnectionDto
                {
                    Price = connection.Price,
                    Time = connection.Time
                };
                graph.Connect((uint)connection.StartCity.CityId, (uint)connection.EndCity.CityId, comparison(connectionDto), connection.ConnectionId);
            }

            var fromCityId = cities
                .First(x => x.CityName == origin)
                .CityId;
            var toCityId = cities
                .First(x => x.CityName == destination)
                .CityId;

            var result = graph.Dijkstra((uint)fromCityId, (uint)toCityId);
            var path = result.GetPath();
            return path;
        }

        public class RouteNode
        {
            public City City { get; set; }
            public int Index { get; set; }
        }
    }

    public static class HttpWebResponseExt
    {
        public static HttpWebResponse GetResponseNoException(this HttpWebRequest req)
        {
            try
            {
                return (HttpWebResponse)req.GetResponse();
            }
            catch (WebException we)
            {
                var resp = we.Response as HttpWebResponse;
                return resp;
            }
        }
    }
}
