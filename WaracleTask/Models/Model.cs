using Microsoft.EntityFrameworkCore;

namespace WaracleTask.Models
{
    public class HotelContext: DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options)
       : base(options)
        {
        }
        public DbSet<HotelRoom> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HotelRoom>()
                .HasData(
                    new HotelRoom { Id = 1, Type = RoomType.Single, RoomCapacity = 1 },
                    new HotelRoom { Id = 2, Type = RoomType.Single, RoomCapacity = 1 },
                    new HotelRoom { Id = 3, Type = RoomType.Double, RoomCapacity = 2 },
                    new HotelRoom { Id = 4, Type = RoomType.Double, RoomCapacity = 2 },
                    new HotelRoom { Id = 5, Type = RoomType.Deluxe, RoomCapacity = 4 },
                    new HotelRoom { Id = 6, Type = RoomType.Deluxe, RoomCapacity = 4 }
                );

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingNumber)
                .IsUnique();
        }
    }

    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<HotelRoom> Rooms { get; set; }
    }

    public class HotelRoom
    {
        public int Id { get; set; }
        public int RoomCapacity { get; set; }
        public RoomType Type { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }

    public enum RoomType
    {
        Single,
        Double,
        Deluxe
    }

    public class Booking
    {
        public int Id { get; set; }
        public required string BookingNumber { get; set; }
        public int RoomId { get; set; }
        public HotelRoom? Room { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestNumbers { get; set; }


    }

}
