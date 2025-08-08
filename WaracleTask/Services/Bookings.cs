using Microsoft.EntityFrameworkCore;
using WaracleTask.Models;

namespace WaracleTask.Services
{
    public class Bookings(HotelContext context)
    {
        private readonly HotelContext _context = context;

        public async Task<string> CreateBooking(string bookingNumber, int roomId, DateTime checkIn, DateTime checkOut, int guests)
        {
            if (!await IsRoomAvailable(roomId, checkIn, checkOut))
                throw new Exception("Room is not available for selected dates");

            if (!await IsCapacityValid(roomId, guests))
                throw new Exception("Number of guests exceeds room capacity");

            var booking = new Booking
            {
                BookingNumber = bookingNumber,
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                GuestNumbers = guests
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking.BookingNumber;
        }

        public async Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return !await _context.Bookings.AnyAsync(b =>
            b.Id == roomId &&
            b.CheckIn == checkIn &&
            b.CheckOut == checkOut);
        }

        public async Task<bool> IsCapacityValid(int roomId, int guests)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            return room != null && guests <= room.RoomCapacity;
        }
    }
}
