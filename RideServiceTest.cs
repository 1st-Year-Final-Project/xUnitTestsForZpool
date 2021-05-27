using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ZPool.Models;
using ZPool.Pages.Rides;
using ZPool.Services.EFServices.RideService;
using ZPool.Services.Interfaces;


namespace xUnitTestProject
{
    public class RideServiceTest : ZPoolTestBase
    {
        private IRideService _rideService;

        public RideServiceTest()
        {
            _rideService = new EFRideService(_context);
            
        }

        [Fact]
        public void GetAllRides_Test()
        {
            // Arrange & Act
            var rides = _rideService.GetAllRides();

            // Assert
            Assert.Equal(3, rides.Count());
        }

        [Fact]
        public void GetRide_Existing_Test()
        {
            Ride ride = _rideService.GetRide(1);

            Assert.Equal(1, ride.RideID);
            Assert.NotNull(ride.Car);
            Assert.NotNull(ride.Car.AppUser);
        }

        [Fact]
        public void DeleteRide_Existing_Test()
        {
            Ride ride = _rideService.GetRide(1);
            _rideService.DeleteRide(ride);

            Assert.Equal(2, _rideService.GetAllRides().Count());
            Assert.Equal(2, _rideService.GetAllRides().ToList()[0].RideID);
        }

        [Fact]
        public void AddNewRide_Valid_Test()
        {
            Ride ride = new Ride()
            {
                RideID = 4,
                CarID = 1,
                DepartureLocation = "Copenhagen",
                DestinationLocation = "Roskilde",
                StartTime = DateTime.Now,
                SeatsAvailable = 4
            };

            _rideService.AddRide(ride);

            Assert.Equal(4, _rideService.GetAllRides().Count());
        }

        [Fact]
        public void UpdateRide_ValidData_Test()
        {
            Ride ride = _rideService.GetRide(1);

            ride.CarID = 2;
            _rideService.EditRide(ride);

            Assert.Equal(2, _rideService.GetRide(1).CarID);
        }

        [Fact]
        public void SearchRide_Success_Test()
        {
            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = "Roskilde",
                DestinationLocation = "Taastrup",
                StartTime = DateTime.Now
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Single(rides);
        }

        [Fact]
        public void SearchRide_Failure_Test()
        {
            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = "Roskilde",
                DestinationLocation = "Albertslund",
                StartTime = DateTime.Now
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Empty(rides);
        }

        [Fact]
        public void SearchRide_ByDepartureOnly_Success_Test()
        {
            // Arrange
            Ride ride = new Ride()
            {
                DepartureLocation = "Stationsvej, Roskilde",
                DestinationLocation = "Halandsbulevard, Taastrup",
                StartTime = DateTime.Now
            };

            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = "Roski",
                DestinationLocation = "",
                StartTime = DateTime.Now
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Single(rides);
        }

        [Fact]
        public void SearchRide_ByDestinationOnly_Success_Test()
        {

            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = "",
                DestinationLocation = "Taast",
                StartTime = DateTime.Now
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Single(rides);
        }

        [Fact]
        public void SearchRide_ByNoLocations_Success_Test()
        {
            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = "",
                DestinationLocation = "",
                StartTime = DateTime.Now
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Equal(3, rides.Count());
        }

        [Fact]
        public void SearchRide_LaterThanStart_Success_Test()
        {
            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = "",
                DestinationLocation = "",
                StartTime = DateTime.Now.Add(new TimeSpan(1,20,0))
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Equal(3, rides.Count());
        }

        [Fact]
        public void SearchRide_EarlierThanStart_Success_Test()
        {
            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = "",
                DestinationLocation = "",
                StartTime = DateTime.Now.Subtract(new TimeSpan(1, 20, 0))
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Equal(3, rides.Count());
        }

        [Fact]
        public void SearchRide_EarlierThanStart_LocationsNull_Success_Test()
        {
            RideCriteriaInputModel search = new RideCriteriaInputModel()
            {
                DepartureLocation = null,
                DestinationLocation = null,
                StartTime = DateTime.Now.Subtract(new TimeSpan(1, 20, 0))
            };

            // Act
            var rides = _rideService.FilterRides(search);

            // Assert
            Assert.Equal(3, rides.Count());
        }

    }
}
