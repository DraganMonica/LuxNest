using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using LuxNest.Application.Common.Interfaces;
using LuxNest.Application.Common.Utility;
using LuxNest.Application.Services.Interface;
using LuxNest.Domain.Entities;
using LuxNest.Web.ViewModels;

namespace LuxNest.Application.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DashboardService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<PieChartDto> GetBookingPieChartData()
        {
            var allBookings = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30)).ToList();

            int approved = allBookings.Count(u => u.Status == SD.StatusApproved);
            int checkedIn = allBookings.Count(u => u.Status == SD.StatusCheckedIn);
            int completed = allBookings.Count(u => u.Status == SD.StatusCompleted);
            int cancelled = allBookings.Count(u => u.Status == SD.StatusCancelled);
            int refunded = allBookings.Count(u => u.Status == SD.StatusRefunded);

            PieChartDto PieChartDto = new()
            {
                Labels = new string[] { "Approved", "Checked In", "Completed", "Cancelled", "Refunded" },
                Series = new decimal[] { approved, checkedIn, completed, cancelled, refunded }
            };

            return PieChartDto;
        }

        public async Task<LineChartDto> GetMemberAndBookingLineChartData()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            var endDate = DateTime.Now.Date;
            var daysInRange = (endDate - startDate).Days + 1;
            var last30Days = Enumerable.Range(0, daysInRange).Select(i => startDate.AddDays(i)).ToList();

            var bookings = _unitOfWork.Booking.GetAll(u => u.BookingDate.Date >= startDate && u.BookingDate.Date <= endDate).ToList();
            var bookingsByDay = bookings.GroupBy(b => b.BookingDate.Date).ToDictionary(g => g.Key, g => g.Count());

            var allUsers = (await _userManager.GetUsersInRoleAsync(SD.Role_Customer)).ToList();
            var usersByDay = allUsers.Where(u => u.CreatedAt.Date >= startDate && u.CreatedAt.Date <= endDate)
                .GroupBy(u => u.CreatedAt.Date).ToDictionary(g => g.Key, g => g.Count());

            var newBookingData = last30Days.Select(d => (int)(bookingsByDay.ContainsKey(d) ? bookingsByDay[d] : 0)).ToArray();
            var newCustomerData = last30Days.Select(d => (int)(usersByDay.ContainsKey(d) ? usersByDay[d] : 0)).ToArray();
            var categories = last30Days.Select(d => d.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> chartDataList = new()
            {
                new ChartData
                {
                    Name="New Bookings",
                    Data=newBookingData
                },
                new ChartData
                {
                    Name="New Customers",
                    Data=newCustomerData
                }
            };

            LineChartDto LineChartDto = new()
            {
                Categories = categories,
                Series = chartDataList
            };

            return LineChartDto;
        }

        public async Task<RadialBarChartDto> GetRegisteredUserChartData()
        {
            var totalUsers = (await _userManager.GetUsersInRoleAsync(SD.Role_Customer)).ToList();

            var countByCurrentMonth = totalUsers.Count(u => u.CreatedAt >= currentMonthStartDate &&
            u.CreatedAt <= DateTime.Now);

            var countByPreviousMonth = totalUsers.Count(u => u.CreatedAt >= previousMonthStartDate &&
            u.CreatedAt <= currentMonthStartDate);

            return SD.GetRadialCartDataModel(totalUsers.Count(), countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialBarChartDto> GetRevenueChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u =>
                u.Status == SD.StatusApproved ||
                u.Status == SD.StatusCheckedIn ||
                u.Status == SD.StatusCompleted);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));

            var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= currentMonthStartDate &&
            u.BookingDate <= DateTime.Now).Sum(u => u.TotalCost);

            var countByPreviousMonth = totalBookings.Where(u => u.BookingDate >= previousMonthStartDate &&
            u.BookingDate <= currentMonthStartDate).Sum(u => u.TotalCost);

            return SD.GetRadialCartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialBarChartDto> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u =>
                u.Status == SD.StatusApproved ||
                u.Status == SD.StatusCheckedIn ||
                u.Status == SD.StatusCompleted);

            var countByCurrentMonth = totalBookings.Count(u => u.BookingDate >= currentMonthStartDate &&
            u.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(u => u.BookingDate >= previousMonthStartDate &&
            u.BookingDate <= currentMonthStartDate);

            return SD.GetRadialCartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth);
        }

      
    }
}
