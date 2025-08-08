using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaracleTask.Models;

namespace WaracleTask.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchHotelsController: ControllerBase
    {
        private readonly HotelContext _context;

        public SearchHotelsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Hotel>>> SearchHotels(string name)
        {
            var hotels = await _context.Hotels
                .Where(hotels => hotels.Name.Contains(name))
                .ToListAsync();

            return hotels;
        }
    }
}
