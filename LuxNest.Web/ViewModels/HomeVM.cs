using LuxNest.Domain.Entities;

namespace LuxNest.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int Nights { get; set; }
        public int NumberOfRooms { get; set; } = 1;
        public int NumberOf2pRooms { get; set; } = 1;
        public int NumberOf3pRooms { get; set; } = 0;
        public int NumberOfGuests { get; set; } = 1;
    }
}
