using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LuxNest.Application.Common.Interfaces;
using LuxNest.Application.Common.Utility;
using LuxNest.Application.Services.Interface;
using LuxNest.Web.Models;
using LuxNest.Web.ViewModels;

namespace LuxNest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IVillaService villaService, IWebHostEnvironment webHostEnvironment)
        {
            _villaService = villaService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(SD.Role_Admin))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            HomeVM homeVM = new()
            {
                VillaList =_villaService.GetAllVillas(),
                Nights=1,
                CheckInDate=DateOnly.FromDateTime(DateTime.Now)
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate, int numberOfRooms = 1, int numberOfOf2pRooms = 0, int numberOfOf3pRooms = 0, int numberOfGuests = 1)
        {
            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                VillaList = _villaService.GetVillasAvailabilityByDate(nights, checkInDate, numberOfOf2pRooms, numberOfOf3pRooms),
                Nights = nights,
                NumberOfRooms = numberOfRooms,
                NumberOf2pRooms = numberOfOf2pRooms,
                NumberOf3pRooms = numberOfOf3pRooms,
                NumberOfGuests = numberOfGuests
            };
            return PartialView("_VillaList", homeVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
