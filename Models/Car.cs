using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCloudLesson1.Models
{
    public class Car
    {
        public String brand { get; set; }
        public String id { get; set; }
        public String model { get; set; }
        public double displacment { get; set; }
        public String color { get; set; }
        public LapTime[] laptimes { get; set; }
    }
}
