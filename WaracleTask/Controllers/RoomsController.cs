using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaracleTask.Models;

namespace WaracleTask.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController: ControllerBase
    {
        private readonly HotelContext _context;

        public RoomsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<HotelRoom>>> GetAvailableRooms(DateTime startDate, DateTime endDate, int guests)
        {
            var rooms = await _context.Rooms
                .Where(r => r.RoomCapacity >= guests &&
                    !r.Bookings.Any(b =>
                    (startDate < b.CheckOut && endDate > b.CheckIn)))
                .ToListAsync();

            return rooms;
        }
    }
}
