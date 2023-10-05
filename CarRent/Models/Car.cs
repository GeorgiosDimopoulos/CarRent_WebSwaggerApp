namespace CarRent.Models
{
    public class Car
    {
        public Car(string name, Location Location, bool Available, int Seats, string Id, EngineType engineType, int carPricePerKlm)
        {
            this.Name = name;
            this.Location = Location;
            this.Available = Available;
            this.Seats = Seats;
            this.Id = Id;
            this.EngineType = engineType;
            this.CarPricePerKlm = carPricePerKlm;
        }

        public string Name { get; set; }

        public Location Location { get; set; }

        public bool Available { get; set; }

        public int Seats { get; set; }

        public int CarPricePerKlm { get; set; }

        public string Id { get; set; }

        public EngineType EngineType { get; set; }
    }

    public enum EngineType
    {
        Diesel,
        Petrol,
        Gas
    }
}