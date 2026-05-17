using LuxNest.Application.Common.Utility;
using LuxNest.Application.Services.Interface;

namespace LuxNest.Web.Services
{
    public class BookingAutoCompleteService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookingAutoCompleteService> _logger;

        public BookingAutoCompleteService(IServiceScopeFactory scopeFactory, ILogger<BookingAutoCompleteService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await AutoCompleteBookings();
                // rulează o dată pe zi
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task AutoCompleteBookings()
        {
            using var scope = _scopeFactory.CreateScope();
            var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

            var today = DateOnly.FromDateTime(DateTime.Now);
            var checkedInBookings = bookingService.GetAllBookings("", SD.StatusCheckedIn);

            foreach (var booking in checkedInBookings)
            {
                if (booking.CheckOutDate < today)
                {
                    bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
                    _logger.LogInformation("Booking {BookingId} auto-completed (checkout was {CheckOut}).",
                        booking.Id, booking.CheckOutDate);
                }
            }

            await Task.CompletedTask;
        }
    }
}
