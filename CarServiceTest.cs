using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ZPool.Models;
using Xunit;
using ZPool.Models;
using ZPool.Services.EFService;
using ZPool.Services.EFService.RideService;
using ZPool.Services.Interface;

namespace xUnitTestProject
{
    public class CarServiceTest : ZPoolTestBase
    {
        private ICarService _carService;
        private IRideService _rideService;

        public CarServiceTest()
        {
            _carService = new EFCarService(base._context);
            _rideService = new RideService(base._context);
        }
        
        [Fact]
        public void GetAllCars_Test()
        {
            // Arrange & Act
            var cars = _carService.GetCars();

            // Assert
            Assert.Equal(3, cars.Count());
            Assert.Equal("Skoda", cars.ToList()[0].Brand);
        }

        [Fact]
        public void GetCar_ExistingId_Test()
        {
            Car car = _carService.GetCar(1);

            Assert.Equal(1, car.CarID);
            Assert.Equal("Skoda", car.Brand);
            Assert.NotNull(car.AppUser);
            Assert.Equal(1, car.AppUserID);
            Assert.Equal(car.AppUser.Id, car.AppUserID);
        }

        [Fact]
        public void GetCar_NotExistingId_ReturnDefault_Test()
        {
            var car = _carService.GetCar(12);

            Assert.Equal(default, car);
        }

        [Fact]
        public void UpdateCar_Valid_Test()
        {
            // Arrange
            Car existingCar = _carService.GetCar(2);

            // Act
            existingCar.Color = "green";
            _carService.UpdateCar(existingCar);

            // Assert
            Assert.Equal("green", _carService.GetCar(2).Color);
        }

        [Fact]
        public void AddCar_WithValidData_Test()
        {
            // Arrange
            Car car = new Car()
            {
                CarID = 4,
                AppUserID = 1,
                Brand = "Opel",
                Model = "Astra",
                Color = "white",
                NumberOfSeats = 5,
                NumberPlate = "SE12345"
            };

            // Act
            _carService.AddCar(car);

            // Assert
            Assert.Equal(4, _carService.GetCars().Count());
            Assert.NotNull(_carService.GetCar(4));
        }

        [Fact]
        public void GetCarsByUser_ExistingUserId_Test()
        {
            IEnumerable<Car> cars = _carService.GetCarsByUser(1);

            Assert.Equal(1, cars.Count());
            Assert.Equal(1, cars.ToList()[0].AppUserID);
        }

        [Fact]
        public void GetCarByUser_InvalidUserId_Test()
        {
            IEnumerable<Car> cars = _carService.GetCarsByUser(12);

            Assert.Equal(0, cars.Count());
        }

        // can't be tested here, a database with stored procedures is required
        //[Fact]
        //public void DeleteCar_ExistingCar_Test()
        //{
        //    _carService.DeleteCar(_carService.GetCar(1));

        //    Assert.Equal(2, _carService.GetCars().Count());
        //    Assert.Equal(2, _rideService.GetAllRides().Count());
        //}

        //[Fact]
        //public void DeleteCar_NotExistingCar_Test()
        //{
        //    Car car = _carService.GetCar(12);
        //    _carService.DeleteCar(car);

        //    Assert.Equal(3, _carService.GetCars().ToList().Count());
        //}

        //[Fact]
        //public void UpdateCar_DefaultCar_Test() // that will never happen
        //{
        //    // Arrange
        //    Car car = _carService.GetCar(12);

        //    // Act
        //    car.Color = "green";
        //    _carService.UpdateCar(car);

        //    // Assert
        //    Assert.Equal(3, _carService.GetCars().Count()); // throws exception
        //}

    }
}
