using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VisioForge.Controls.UI.Annotations;
using Xunit;
using ZPool.Models;
using ZPool.Services.EFServices;
using ZPool.Services.Interfaces;


namespace xUnitTestProject
{
    public class BookingServiceTest : ZPoolTestBase
    {
        private IBookingService _bookingService;

        public BookingServiceTest()
        {
            _bookingService = new EFBookingService(base._context, new EFMessageService(_context));
        }

        [Fact]
        public void GetAllBookings_Test()
        {
            var list = _bookingService.GetBookings();

            Assert.NotNull(list);
            Assert.Equal(3, list.Count());
        }

        [Fact]
        public void GetBooking_Test()
        {
            var booking = _bookingService.GetBookingsByID(1);

            Assert.IsType<Booking>(booking);
            Assert.Equal(1, booking.BookingID);
        }

        [Fact]
        public void GetBookingsByUserId_Test()
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == 2);
            var bookings = _bookingService.GetBookingsByUser(user);

            Assert.Equal(2, bookings.Count());
            Assert.Equal(2, bookings.ToList()[0].AppUserID);
        }

        [Fact]
        public void GetBookingsByRideId_Test()
        {
            var bookings = _bookingService.GetBookingsByRideId(1);

            Assert.Equal(2, bookings.Count());
            Assert.Equal(1, bookings.ToList()[0].RideID);
            Assert.IsType<Booking>(bookings.ToList()[1]);
        }

        [Fact]
        public void AddBooking_Success_Test()
        {
            Booking booking = new Booking()
            {
                AppUserID = 3,
                BookingID = 4,
                BookingStatus = "Pending",
                Date = DateTime.Now,
                PickUpLocation = "",
                DropOffLocation = "",
                RideID = 1
            };

            _bookingService.AddBooking(booking);

            Assert.Equal(4, _bookingService.GetBookings().Count());
        }

        [Fact]
        public void AddBooking_WithExistingBooking_Failure_Test()
        {
            Booking booking = new Booking()
            {
                AppUserID = 2,
                BookingID = 4,
                BookingStatus = "Pending",
                Date = DateTime.Now,
                PickUpLocation = "",
                DropOffLocation = "",
                RideID = 1
            };

            _bookingService.AddBooking(booking);

            Assert.Equal(3, _bookingService.GetBookings().Count());
        }

        [Fact]
        public void UpdateBookingStatus_Test()
        {
            string oldStatus = _bookingService.GetBookingsByID(2).BookingStatus;
            _bookingService.UpdateBookingStatus(2, "Accepted");
            string newStatus = _bookingService.GetBookingsByID(2).BookingStatus;

            Assert.NotEqual(oldStatus, newStatus );
            Assert.Equal("Accepted", newStatus);

        }

        [Fact]
        public void AlreadyBooked_Accepted_SameBooking_Test()
        {
            var check = _bookingService.AlreadyBooked(1, 1);
            Assert.True(check);
        }

        [Fact]
        public void AlreadyBooked_Pending_SameBooking_Test()
        {
            var check = _bookingService.AlreadyBooked(1, 2);
            Assert.True(check);
        }

        [Fact]
        public void AlreadyBooked_Pending_OtherBooking_Test()
        {
            var check = _bookingService.AlreadyBooked(1, 3);
            Assert.False(check);
        }

        [Fact]
        public void AlreadyBooked_Rejected_SameBooking_False_Test()
        {
            var check = _bookingService.AlreadyBooked(3, 2);
            Assert.False(check);
        }

        [Fact]
        public void GetBookingsByStatus_Test()
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.Id == 2);

            var bookings = _bookingService.GetBookingsByStatus("Accepted", user);
            var bookings2 = _bookingService.GetBookingsByStatus("Pending", user);
            var bookings3 = _bookingService.GetBookingsByStatus("Rejected", user);
            var bookings4 = _bookingService.GetBookingsByStatus("Cancelled", user);

            Assert.Empty(bookings);
            Assert.Single(bookings2);
            Assert.Single(bookings3);
            Assert.Empty(bookings4);
        }

        [Fact]
        public void UpdateBookingStatus_AcceptedToAcceptedThrowsEx_Test()
        {
            Assert.ThrowsAny<Exception>(()=> _bookingService.UpdateBookingStatus(1, "Accepted"));
        }

        [Fact]
        public void UpdateBookingStatus_RejectedToAccepted_ThrowsEx_Test()
        {
            Assert.ThrowsAny<Exception>(() => _bookingService.UpdateBookingStatus(3, "Accepted"));
        }

        [Fact]
        public void UpdateBookingStatus_RejectedToPending_ThrowsEx_Test()
        {
            Assert.ThrowsAny<Exception>(() => _bookingService.UpdateBookingStatus(3, "Pending"));
        }

        [Fact]
        public void UpdateBookingStatus_AcceptedToRejected_ThrowsEx_Test()
        {
            Assert.ThrowsAny<Exception>(() => _bookingService.UpdateBookingStatus(1, "Rejected"));
        }
    }
}
