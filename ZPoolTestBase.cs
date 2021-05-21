using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UserManagementTestApp.Models;
using ZPool.Models;
using ZPool.Models;

namespace xUnitTestProject
{
    public class ZPoolTestBase
    {
        protected AppDbContext _context;

        public ZPoolTestBase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite($"Filename={Guid.NewGuid()}.db")
                .Options;

            _context = new AppDbContext(options);

            Seed();
        }

        private void Seed()
        {
            // Here the test data is seeded to the test database.
            // You can add more data, but you have to attend to referential integrity, else exception.
            // If new data is added, it might change the outcome of previous tests (e.g. Count() of items in a list)

            
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();

                #region List of Users

                _context.Users.Add(new AppUser()
                {
                    Id = 1,
                    Email = "user@email.com",
                    FirstName = "Tester",
                    LastName = "Master",
                    UserName = "TestMaster"
                });
                _context.Users.Add(new AppUser()
                {
                    Id = 2,
                    Email = "user2@email.com",
                    FirstName = "Another",
                    LastName = "Tester",
                    UserName = "TesterNo2"
                });
                _context.Users.Add(new AppUser()
                {
                    Id = 3,
                    Email = "user3@email.com",
                    FirstName = "AnotherOne",
                    LastName = "Tester",
                    UserName = "TesterNo3"
                });
            #endregion


            #region List of Cars

            _context.Cars.Add(new Car()
                {
                    CarID = 1,
                    AppUserID = 1,
                    Brand = "Skoda",
                    Model = "CityGo",
                    Color = "red",
                    NumberOfSeats = 5,
                    NumberPlate = "AB12345"
                });

                _context.Cars.Add(new Car()
                {
                    CarID = 2,
                    AppUserID = 2,
                    Brand = "Volvo",
                    Model = "S60",
                    Color = "gray",
                    NumberOfSeats = 5,
                    NumberPlate = "XY12345"
                });

                _context.Cars.Add(new Car()
                {
                    CarID = 3,
                    AppUserID = 3,
                    Brand = "Peugeot",
                    Model = "206",
                    Color = "black",
                    NumberOfSeats = 5,
                    NumberPlate = "CD12345"
                });


                #endregion


                #region List of Rides

                _context.Rides.Add(new Ride()
                {
                    RideID = 1,
                    CarID = 1,
                    DepartureLocation = "Copenhagen",
                    DestinationLocation = "Roskilde",
                    StartTime = DateTime.Now,
                    SeatsAvailable = 3
                });

                _context.Rides.Add(new Ride()
                {
                    RideID = 2,
                    CarID = 2,
                    DepartureLocation = "Slagelse",
                    DestinationLocation = "Roskilde",
                    StartTime = DateTime.Now,
                    SeatsAvailable = 3
                });

                _context.Rides.Add(new Ride()
                {
                    RideID = 3,
                    CarID = 3,
                    DepartureLocation = "Roskilde",
                    DestinationLocation = "Taastrup",
                    StartTime = DateTime.Now,
                    SeatsAvailable = 3
                });

            #endregion

            #region List of Bookings

            _context.Bookings.Add(new Booking()
            {
                BookingID = 1,
                BookingStatus = "Accepted",
                AppUserID = 1,
                RideID = 1,
                Date = DateTime.Now,
                PickUpLocation = "",
                DropOffLocation = ""
            });

            _context.Bookings.Add(new Booking()
            {
                BookingID = 2,
                BookingStatus = "Pending",
                AppUserID = 2,
                RideID = 1,
                Date = DateTime.Now,
                PickUpLocation = "",
                DropOffLocation = ""
            });

            _context.Bookings.Add(new Booking()
            {
                BookingID = 3,
                BookingStatus = "Rejected",
                AppUserID = 2,
                RideID = 3,
                Date = DateTime.Now,
                PickUpLocation = "",
                DropOffLocation = ""
            });

            #endregion


            _context.SaveChanges();
            
        }
    }
}
