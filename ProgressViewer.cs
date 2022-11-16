using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using System.Runtime.CompilerServices;

namespace ConsoleApp1
{
    internal class ProgressViewer
    {
        public int Progress { get; set; }
        public int Total { get; set; }

        private readonly object ProgressLock = new object();

        public ProgressViewer(int total)
        {
            Progress = 0;
            Total = total;
        }

        public void OnPlanetDeserialized()
        {
            lock (this.ProgressLock)
            {
                this.Progress++;
                int pourcentage = Progress * 100 / Total;
                Console.Write("\r{0}%   ", pourcentage);
            }
        }
    }
}
