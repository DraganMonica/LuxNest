using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LuxNest.Domain.Entities;

namespace LuxNest.Infrastructure.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        //ctor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas{ get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Description = "Nestled among lush tropical gardens, the Royal Villa offers a serene escape with a private pool, sun-drenched balcony, and a warm king-size bedroom. Ideal for couples or small families seeking nature, privacy, and charm.",
                    ImageUrl = "/images/VillaImage/royal_villa_card.png",
                    Occupancy = 4,
                    Price = 200,
                    Sqft = 550,
                },
                new Villa
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Description = "An architectural showpiece that comes alive at night, the Premium Pool Villa features a stunning illuminated pool, spacious interiors, and flexible sleeping arrangements for up to 4 guests. Perfect for those who love to entertain under the stars.",
                    ImageUrl = "/images/VillaImage/premium_pool_villa_card.png",
                    Occupancy = 4,
                    Price = 300,
                    Sqft = 550,
                },
                new Villa
                {
                    Id = 3,
                    Name = "Luxury Pool Villa",
                    Description = "The pinnacle of modern luxury. Floor-to-ceiling glass walls frame breathtaking panoramic views, while a private pool and jacuzzi await just outside. With 750 sqft of refined living space, the Luxury Pool Villa is an unparalleled retreat for the discerning traveller.",
                    ImageUrl = "/images/VillaImage/luxury_pool_villa_card.png",
                    Occupancy = 4,
                    Price = 400,
                    Sqft = 750,
                });
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 101,
                    VillaId = 1
                },
                new VillaNumber
                {
                    Villa_Number = 102,
                    VillaId = 1
                },
                new VillaNumber
                {
                    Villa_Number = 103,
                    VillaId = 1
                },
                new VillaNumber
                {
                    Villa_Number = 104,
                    VillaId = 1
                },
                new VillaNumber
                {
                    Villa_Number = 201,
                    VillaId = 2
                },
                new VillaNumber
                {
                    Villa_Number = 202,
                    VillaId = 2
                },
                new VillaNumber
                {
                    Villa_Number = 301,
                    VillaId = 3
                },
                new VillaNumber
                {
                    Villa_Number = 302,
                    VillaId = 3
                },
                new VillaNumber
                {
                    Villa_Number = 303,
                    VillaId = 3
                });
            modelBuilder.Entity<Amenity>().HasData(
                  new Amenity
                  {
                      Id = 1,
                      VillaId = 1,
                      Name = "Private Pool"
                  }, 
                  new Amenity
                  {
                      Id = 2,
                      VillaId = 1,
                      Name = "Microwave"
                  }, 
                  new Amenity
                  {
                      Id = 3,
                      VillaId = 1,
                      Name = "Private Balcony"
                  }, 
                  new Amenity
                  {
                      Id = 4,
                      VillaId = 1,
                      Name = "1 king bed and 1 sofa bed"
                  },
                  new Amenity
                  {
                      Id = 5,
                      VillaId = 2,
                      Name = "Private Plunge Pool"
                  }, 
                  new Amenity
                  {
                      Id = 6,
                      VillaId = 2,
                      Name = "Microwave and Mini Refrigerator"
                  }, 
                  new Amenity
                  {
                      Id = 7,
                      VillaId = 2,
                      Name = "Private Balcony"
                  }, 
                  new Amenity
                  {
                      Id = 8,
                      VillaId = 2,
                      Name = "king bed or 2 double beds"
                  },
                  new Amenity
                  {
                      Id = 9,
                      VillaId = 3,
                      Name = "Private Pool"
                  }, 
                  new Amenity
                  {
                      Id = 10,
                      VillaId = 3,
                      Name = "Jacuzzi"
                  }, 
                  new Amenity
                  {
                      Id = 11,
                      VillaId = 3,
                      Name = "Private Balcony"
                  });
        }
    }
}
