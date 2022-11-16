using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    internal class SolarSystem
    {
        public List<Planet> Planets { get; set; }

        public SolarSystem(List<Planet> planets)
        {
            Planets = planets;
        }
    }
}
