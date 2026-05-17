using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxNest.Application.Common.Interfaces;
using LuxNest.Application.Services.Implementation;
using LuxNest.Application.Services.Interface;
using LuxNest.Domain.Entities;
using LuxNest.Infrastructure.Data;
using LuxNest.Web.ViewModels;

namespace LuxNest.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;

        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            // we want to retrieve all the villas from the database
            var villanumbers = _villaNumberService.GetAllVillaNumbers();
            return View(villanumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVm = new()
            {
                //ce e populat aici, trebuie populat si mai jos: AICI
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _villaNumberService.CheckVillaNumberExists(obj.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                _villaNumberService.CreateVillaNumber(obj.VillaNumber);
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index");
            }

            if (roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists.";
            }
            // AICI!
            obj.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVm = new()
            {
                //ce e populat aici, trebuie populat si mai jos: AICI
                VillaList = _villaService.GetAllVillas().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(villaNumberId)
            };
            if (villaNumberVm.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
           
            if (ModelState.IsValid )
            {
                _villaNumberService.UpdateVillaNumber(villaNumberVM.VillaNumber);
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction("Index");
            }

            villaNumberVM.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVm = new()
            {
                //ce e populat aici, trebuie populat si mai jos: AICI
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(villaNumberId)
            };
            if (villaNumberVm.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _villaNumberService.GetVillaNumberById(villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _villaNumberService.DeleteVillaNumber(objFromDb.Villa_Number);
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index");

            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
