namespace CarRent.Models
{
    public class Reservation
    {
        public Reservation()
        {
        }

        public Reservation(Location pickUpLocation, Location dropOffLocation, DateTime pickupTime, DateTime dropoffTime, int price, Person customer, Car car)
        {
            this.DropOffTime = dropoffTime;
            this.PickUpTime = pickupTime;
            this.DropOffLocation = dropOffLocation;
            this.PickUpLocation = pickUpLocation;
            this.RentPrice = price;
            this.Customer = customer;
            this.Car = car;
        }

        public Location PickUpLocation { get; set; }

        public Location DropOffLocation { get; set; }

        public DateTime PickUpTime { get; set; }

        public DateTime DropOffTime { get; set; }

        public int RentPrice { get; set; }

        public Car Car { get; set; }

        public Person Customer { get; set; }
    }
}