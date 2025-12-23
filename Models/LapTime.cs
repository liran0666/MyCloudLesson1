using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace MyCloudLesson1.Models
{
    public class LapTime
    {
        public double time { get; set; }
        public override string ToString()
        {
            return $"{time} seconds";
        }
    }
    
}