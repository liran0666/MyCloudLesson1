namespace CloudApp.Models
{
    public class Adress
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNum { get; set; }

        public override string ToString()
        {
            string addrDataStr = string.Empty;
            addrDataStr += (string.IsNullOrEmpty(City)) ?
                               "\nNo city documented at the moment" :
                               "\ncity:" + City;
            addrDataStr += (string.IsNullOrEmpty(Street)) ?
                               "\nNo street documented at the moment" :
                               "\nstreet:" + Street;
            addrDataStr += $"\nHouse num: {HouseNum}";
            return addrDataStr;
        }
    }

}