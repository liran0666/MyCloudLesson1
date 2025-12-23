using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyCloudLesson1.Models
{
    public class Car
    {

        public String brand { get; set; }
        public String id { get; set; }
        public String ObjType { get; set; }
        public String model { get; set; }
        public double displacment { get; set; }
        public String color { get; set; }
        public LapTime[] laptimes { get; set; }
        public Car()
        {
            ObjType = GetType().Name;
        }
        public override string ToString()
        {

            string carInfo = "";
            carInfo+= ($" Brand: {brand}");
            carInfo += ($" ID: {id}");
            carInfo += ($" Model: {model}");
            carInfo += ($" Displacement: {displacment}L");
            carInfo +=  ($" Color: {color}");

            
            if (laptimes != null && laptimes.Length > 0)
            {
                carInfo += (" Lap Times:");
                foreach (var lap in laptimes)
                {
                    carInfo += ($"- {lap}");
                }
            }
            else
            {
                carInfo += ("No lap times available.");
            }

            // If there are passengers, include them in the string representation



            return carInfo;
        }
        
    }
    
}
