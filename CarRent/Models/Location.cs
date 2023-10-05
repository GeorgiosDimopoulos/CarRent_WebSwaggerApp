namespace CarRent.Models
{
    public class Location
    {
        public Location(string Address, int? x = 0, int? y = 0)
        {
            this.X = x;
            this.Y = y;
            this.Address = Address;
        }

        public int? X { get; set; }

        public int? Y { get; set; }

        public string? Address { get; set; }

    }
}
