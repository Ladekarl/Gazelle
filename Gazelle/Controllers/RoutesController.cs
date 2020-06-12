using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using Gazelle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public class ComparisonDto
        {
            public int Value;
            public string Company;
        }

        public class ConnectionComparisonDto
        {
            public int? Price { get; set; }
            public int? Time { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Route>>> Get(string origin, string destination, int weight, int length, int height, int depth, string deliveryTypes)
        {
            if (weight > 40)
            {
                return NotFound();
            }

            var cheapestCompaniesUsed = new List<string>();
            var cheapestRoute = await CalculateCheapestRoute(origin, destination, c =>
            {
                var prices = new List<ComparisonDto>();
                var eastIndia = GetFromRemote("http://wa-eitdk.azurewebsites.net/api/getshippinginfo", c.Origin, c.Destination, weight, length, height, depth, deliveryTypes);
                var oceanic = GetFromRemote("http://wa-oadk.azurewebsites.net/api/routeApi", c.Origin, c.Destination, weight, length, height, depth, deliveryTypes);

                if (eastIndia != null)
                {
                    prices.Add(new ComparisonDto
                    {
                        Value = Convert.ToInt32(eastIndia.Price.Value),
                        Company = "East India"
                    });
                }

                if (oceanic != null)
                {
                    prices.Add(new ComparisonDto
                    {
                        Value = Convert.ToInt32(oceanic.Price.Value),
                        Company = "Oceanic"
                    });
                }

                if (c.Price.HasValue)
                {
                    prices.Add(new ComparisonDto
                    {
                        Value = c.Price.Value,
                        Company = "Telstar"
                    });
                }
                if (prices.Count() > 0)
                {
                    return prices.Min(c => c.Value);
                }
                return 10000;
            }, deliveryTypes, weight, length, height, depth);

            var shortestCompaniesUsed = new List<string>();
            var shortestRoute = await CalculateShortestRoute(origin, destination, c =>
            {
                var times = new List<ComparisonDto>();
                ConnectionDto eastIndia = null;
                ConnectionDto oceanic = null;
                try
                {
                    eastIndia = GetFromRemote("http://wa-eitdk.azurewebsites.net/api/getshippinginfo", origin, destination, weight, length, height, depth, deliveryTypes);
                }
                catch (Exception ex)
                {
                    //Ignored
                };
                try
                {
                    oceanic = GetFromRemote("http://wa-oadk.azurewebsites.net/api/routeApi", origin, destination, weight, length, height, depth, deliveryTypes);
                }
                catch (Exception ex)
                {
                    //Ignored
                }
                if (eastIndia != null)
                {
                    times.Add(new ComparisonDto
                    {
                        Value = Convert.ToInt32(eastIndia.Time.Value),
                        Company = "East India"
                    });
                }

                if (oceanic != null)
                {
                    times.Add(new ComparisonDto
                    {
                        Value = Convert.ToInt32(oceanic.Time.Value),
                        Company = "Oceanic"
                    });
                }
                if (c.Time.HasValue)
                {
                    times.Add(new ComparisonDto
                    {
                        Value = c.Time.Value,
                        Company = "Telstar"
                    });
                }

                if (times.Count() > 0)
                {
                    return times.Min(c => c.Value);
                }
                return 10000;
            }, deliveryTypes, weight, length, height, depth);

            if (cheapestRoute == null || shortestRoute == null)
            {
                return NotFound();
            }

            cheapestRoute.Connections = null;
            shortestRoute.Connections = null;

            return new List<Route> { cheapestRoute, shortestRoute };
        }

        private async Task<Route> CalculateCheapestRoute(string origin, string destination, Func<ConnectionComparisonDto, int> comparison, string deliveryTypes, int weight, int length, int height, int depth)
        {
            var connections = await _context.Connections
                .Include(c => c.StartCity).ThenInclude(c => c.Country)
                .Include(c => c.EndCity).ThenInclude(c => c.Country)
                .ToListAsync();
            var result = await CalculateRoute(origin, destination, connections, comparison);
            var path = result.GetPath();
            if (path.Count() >= 2)
            {
                return await GetRouteFromPath(result, connections, deliveryTypes, false, weight, length, height, depth);
            }

            return null;
        }

        private async Task<Route> GetRouteFromPath(ShortestPathResult result, ICollection<Connection> connections, string deliveryTypes, bool isTime, int weight, int length, int height, int depth)
        {
            var resultConnections = new List<Connection>();
            var path = result.GetPath();
            for (int i = 1; i < path.Count(); i++)
            {
                var firstElement = path.ElementAt(i - 1);
                var secondElement = path.ElementAt(i);
                var edgeConnections = connections
                    .Where(c => c.StartCity.CityId == firstElement && c.EndCity.CityId == secondElement)
                    .OrderBy(c => c.Price)
                    .ToList();

                foreach (var edgeConnection in edgeConnections)
                {

                    var firstElementName = _context.Cities.Find(Convert.ToInt32(firstElement));
                    var secondElementName = _context.Cities.Find(Convert.ToInt32(secondElement));
                    var elements = new List<ConnectionDto>();
                    var eastIndia = GetFromRemote("http://wa-eitdk.azurewebsites.net/api/getshippinginfo", firstElementName.CityName, secondElementName.CityName, weight, length, height, depth, deliveryTypes);
                    var oceanic = GetFromRemote("http://wa-oadk.azurewebsites.net/api/routeApi", firstElementName.CityName, secondElementName.CityName, weight, length, height, depth, deliveryTypes);
                    if (eastIndia != null)
                    {
                        elements.Add(eastIndia);
                    }
                    if (oceanic != null)
                    {
                        elements.Add(oceanic);
                    }
                    elements.Add(new ConnectionDto
                    {
                        Price = edgeConnection.Price,
                        Time = edgeConnection.Time
                    });
                    if (elements.Count > 0)
                    {
                        if (isTime)
                        {
                            elements.OrderBy(c => c.Time);
                        }
                        else
                        {
                            elements.OrderBy(c => c.Price);
                        }
                        var first = elements.Where(c=> c.Price > 0 && c.Time > 0).First();
                        edgeConnection.Price = Convert.ToInt32(first.Price);
                        edgeConnection.Time = Convert.ToInt32(first.Time);
                    }
                }

                if (isTime)
                {
                    edgeConnections.OrderBy(c => c.Time);
                }
                else
                {
                    edgeConnections.OrderBy(c => c.Price);
                }
                var firstEdge = edgeConnections.Where(c => c.Price > 0 && c.Time > 0).First();

                resultConnections.Add(firstEdge);
            }

            bool hasWar = resultConnections.Any(c => c.EndCity.Country.Conflict || c.StartCity.Country.Conflict);

            var calcPrice = resultConnections.Where(c => c.Price.HasValue).Sum(c => c.Price.Value);
            var calcTime = resultConnections.Where(c => c.Time.HasValue).Sum(c => c.Time.Value);

            var sentTypes = new List<DeliveryType>();

            if (deliveryTypes != null)
            {
                var deliveryTypesArray = deliveryTypes.Split(',');

                foreach (var deliveryType in deliveryTypesArray)
                {
                    if (!string.IsNullOrEmpty(deliveryType))
                    {
                        var dbType = _context.DeliveryTypes.First(x => x.Name == deliveryType);
                        sentTypes.Add(dbType);
                    }
                }
            }

            if (hasWar)
            {
                var warDelivery = _context.DeliveryTypes.First(x => x.Name == "war");
                if (!sentTypes.Contains(warDelivery))
                {
                    sentTypes.Add(warDelivery);
                }
            }

            var totalPriceAddition = sentTypes.Sum(x => x.Price);

            var route = new Route
            {
                Price = !isTime ? result.Distance : calcPrice + (calcPrice * (totalPriceAddition / 100)),
                Time = isTime ? result.Distance : calcTime,
                Companies = "Telstar",
                Connections = resultConnections
            };

            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        private async Task<Route> CalculateShortestRoute(string origin, string destination, Func<ConnectionComparisonDto, int> comparison, string deliveryTypes, int weight, int length, int height, int depth)
        {
            var connections = await _context.Connections.ToListAsync();
            var result = await CalculateRoute(origin, destination, connections, comparison);
            var path = result.GetPath();
            if (path.Count() >= 2)
            {
                return await GetRouteFromPath(result, connections, deliveryTypes, true, weight, length, height, depth);
            }

            return null;
        }

        private ConnectionDto GetFromRemote(string url, string origin, string destination, int weight, int length, int height, int depth, string deliveryTypes)
        {
            WebRequest request = WebRequest.Create(string.Format("{0}?origin={1}&destination={2}&weight={3}&length={4}&width={5}&depth={6}&deliverytypes={7}", url, origin, destination, weight, length, height, depth, deliveryTypes));
            request.Method = "GET";

            HttpWebResponse response = ((HttpWebRequest)request).GetResponseNoException();

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ConnectionDto>(responseFromServer, new JsonSerializerSettings
                {
                    Culture = CultureInfo.InvariantCulture
                });
            }
            return null;
        }

        private async Task<ShortestPathResult> CalculateRoute(string origin, string destination, IEnumerable<Connection> connections, Func<ConnectionComparisonDto, int> comparison)
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
                var connectionDto = new ConnectionComparisonDto
                {
                    Price = connection.Price,
                    Time = connection.Time,
                    Origin = connection.StartCity.CityName,
                    Destination = connection.EndCity.CityName
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
            return result;
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
