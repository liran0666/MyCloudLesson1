using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCloudLesson1.Models
{
    public class Driver
    {
        
        public String id { get; set; }
        public String driverName { get; set; }
        public int yearsInService { get; set; }
        public double age { get; set; }
        public Passengers[] passengers { get; set; }
        public CabStations[] stations { get; set; }

    }
}
