using CarRent.Models;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarController : ControllerBase
    {
        private List<Car> CarList = new List<Car>();
        private List<Location> LocationList = new List<Location>();
        private List<Person> customerList = new List<Person>();
        private List<string> userDemands = new List<string>();

        private readonly ILogger<CarController> _logger;

        public CarController(ILogger<CarController> logger)
        {
            userDemands = new List<string>();

            CarList = new List<Car>
            {
                new Car("Yaris", new Location("Gewerbestrasse 11"), true, 5,"123213", EngineType.Diesel, 5),
                new Car("X2", new Location("Neustrasse 10", 434343,232123), true, 5,"232323", EngineType.Diesel, 5),
                new Car("Golf",  new Location("Neustrasse 12"), false, 3,"321321", EngineType.Petrol, 3),
                new Car("Kompressor",  new Location("Schwabstrasee 11",111111,123125), false, 5,"111111", EngineType.Gas, 2),
            };

            LocationList = new List<Location>
            {
                new Location("Gewerbestrasse 11"),
                new Location("Neustrasse 12"),
            };

            customerList = new List<Person>
            {
                new Person("1111", "George Dim", 30, Gender.Male, UserType.Customer),
                new Person("21312", "Entzi Kourti", 32, Gender.Female, UserType.Customer)
            };

            _logger = logger;
        }


        [HttpPost(Name = "AddDemand")]
        public void AddDemand(string demand)
        {
            userDemands.Add(demand);
        }

        // add a reservation with particular fields
        [HttpPost(Name = "AddReservation")]
        public Car AddReservation(
            string customerId,
            string carId,
            string pickUpLocation,
            string dropOffLocation,
            string pickUpDateTime,
            string dropoffDateTime)
        {
            Person customer = GetCustomer(customerId);
            customer.User = UserType.Customer;

            if (customer == null)
            {
                _logger.LogDebug($"{customer} not found on database with id" + customerId);
                return null;
            }

            _logger.LogDebug($"{customer} found on database with id" + customerId);

            Car car = GetCar(carId);
            if (car == null)
            {
                _logger.LogDebug($"{car} not found on database with id" + carId);
                return null;
            }

            _logger.LogDebug($"{car} found on database with id" + carId);

            car.Available = false;

            var pickUpLoc = LocationList.First(l => l.Equals(pickUpLocation));
            var dropOffLoc = LocationList.First(l => l.Equals(dropOffLocation));

            if (dropOffLoc == null || pickUpLoc == null)
            {
                _logger.LogDebug($"one of the given locations does not exist in our system, sorry!");
                return null;
            }

            try
            {
                var dropOffTime = DateTime.Parse(dropoffDateTime);
                var pickUpTime = DateTime.Parse(pickUpDateTime);
                Reservation reservation = new Reservation()
                {
                    PickUpLocation = pickUpLoc,
                    DropOffLocation = dropOffLoc,
                    PickUpTime = pickUpTime,
                    DropOffTime = dropOffTime,
                    RentPrice = car.CarPricePerKlm,
                    Customer = customer,
                    Car = car
                };
                _logger.LogDebug($"Reservation {reservation} made for {customer} and {car}, pick up place: {pickUpLocation} and drop off place: {dropOffLocation}");
            }
            catch
            {
                _logger.LogDebug($"the proper date time standard is: 2018-03-20T09:12:28Z");
                return null;
            }

            return car;
        }

        // get all the locations that the company has car parkings
        [HttpGet(Name = "GetLocations")]
        private IEnumerable<Location> GetLocations()
        {
            return LocationList;
        }

        // get a specific location and check if the company has car parking there
        [HttpGet(Name = "GetLocation")]
        private Location GetLocation(string locStr, string x, string y)
        {
            var loc = new Location(locStr, int.Parse(x), int.Parse(y));
            return LocationList.First(l => l.X == loc.X && l.Y == loc.Y);
        }

        // try to remove a car, if it exists and is free
        [HttpDelete(Name = "RemoveCar")]
        private void RemoveCar(string carId)
        {
            var car = CarList.First(car => car.Id.Equals(carId));
            if (car.Available == true)
            {
                CarList.Remove(car);
                _logger.LogDebug($"{car} cannot remove from database, it is use");
            }

            _logger.LogDebug($"{car} removed from the list");
        }

        // by giving a GPS point, find the closest car
        [HttpDelete(Name = "GetClosestCar")]
        private void GetClosestCar(string X, string Y)
        {
            var car = CarList [new Random().Next(0, CarList.Count)];

            var coord = new GeoCoordinate(double.Parse(X), double.Parse(Y));
            var nearestPoint = LocationList.Select(loc => new GeoCoordinate((double)loc.X, (double)loc.Y)).OrderBy(x => x.GetDistanceTo(coord)).First();

            foreach (var tempCar in CarList)
            {
                if (tempCar.Available == true && tempCar.Location.X == nearestPoint.Latitude && tempCar.Location.Y == nearestPoint.Longitude)
                {
                    CarList.Remove(car);
                    _logger.LogDebug($"closest {car} is in {coord} location");
                }
            }
        }

        [HttpGet(Name = "GetCustomer")]
        private Person GetCustomer(string userId)
        {
            return (Person)customerList.Where(person => person.Id.Equals(userId));
        }

        [HttpGet(Name = "GetAllCars")]
        public IEnumerable<Car> GetAllCars()
        {
            return CarList;
        }

        [HttpGet(Name = "GetCar")]
        public Car GetCar(string carId)
        {
            return CarList.First(car => car.Id.Equals(carId));
        }
    }
}