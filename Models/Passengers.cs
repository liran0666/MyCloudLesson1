namespace MyCloudLesson1.Models
{
    public class Passengers
    {
        public string name { get; set; }
        public string specialRequest { get; set; }
        public double age { get; set; }
        public override string ToString()
        {
            return $"{name} (Age: {age}, Request: {specialRequest})";
        }
    }
    
}