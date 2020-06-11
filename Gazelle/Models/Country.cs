using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazelle.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public bool Conflict { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
