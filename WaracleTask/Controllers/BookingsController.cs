using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaracleTask.Models;

namespace WaracleTask.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingsController: ControllerBase
    {
        private readonly HotelContext _context;

        public BookingsController(HotelContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            var room = await _context.Rooms.FindAsync(booking.RoomId);
            if (room == null || room.RoomCapacity < booking.GuestNumbers)
                return BadRequest("Invalid room or guest count");

            bool isBooked = await _context.Bookings.AnyAsync(b =>
                b.RoomId == booking.RoomId &&
                booking.CheckIn < b.CheckOut &&
                booking.CheckIn > b.CheckOut);

            if (isBooked)
                return Conflict("Room is already booked for these dates.");

            var bookingInformation = new Booking
            {
                RoomId = booking.RoomId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestNumbers = booking.GuestNumbers,
                BookingNumber = Guid.NewGuid().ToString().Substring(0, 8)
            };

            _context.Bookings.Add(bookingInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookingByReference), new { reference = booking.BookingNumber }, booking);
        }

        [HttpGet("{reference}")]
        public async Task<ActionResult<Booking>> GetBookingByReference(string reference)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.BookingNumber == reference);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }
    }
}
