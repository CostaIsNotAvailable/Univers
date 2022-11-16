using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    internal delegate void PlanetDeserializeEventHandler();
    internal class Planet
    {
        public int Size { get; set; }
        public int Usability { get; set; }
        public int Orbit { get; set; }
        public string Name { get; set; }

        public event PlanetDeserializeEventHandler Deserialized;

        public Planet(int size, int usability, int orbit, string name)
        {
            this.Size = size;
            this.Usability = usability;
            this.Orbit = orbit;
            this.Name = name;
        }

        public virtual void OnDeserialized()
        {
            Deserialized?.Invoke();
        }
    }
}
