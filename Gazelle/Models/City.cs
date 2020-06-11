using System;
using System.Collections;
using System.Collections.Generic;

namespace Gazelle.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public Country Country { get; set; }
    }
}
