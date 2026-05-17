using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuxNest.Application.Common.Interfaces;
using LuxNest.Application.Common.Utility;
using LuxNest.Application.Services.Interface;
using LuxNest.Domain.Entities;
namespace LuxNest.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _webHostEnvironment;


        public VillaService(IUnitOfWork unitOfWork, IHostingEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public void CreateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");
                using var filseStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                villa.Image.CopyTo(filseStream);

                villa.ImageUrl = @"\images\VillaImage\" + fileName;

            }
            else
            {
                villa.ImageUrl = "https://placehold.co/600x400";
            }
            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Save();
        }

        public bool DeleteVilla(int id)
        {
            try { 
            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    _unitOfWork.Villa.Remove(objFromDb);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumbers = _unitOfWork.VillaNumber.GetAll().ToList();
            foreach (var villa in villaList)
            {
                var rooms = villaNumbers.Where(vn => vn.VillaId == villa.Id).ToList();
                villa.TotalCapacity = rooms.Sum(vn => vn.Capacity);
                villa.Rooms2p = rooms.Count(vn => vn.Capacity <= 2);
                villa.Rooms3p = rooms.Count(vn => vn.Capacity >= 3);
            }
            return villaList;
        }

        public Villa GetVillaById(int id)
        {
            var villa = _unitOfWork.Villa.Get(u => u.Id == id, includeProperties: "VillaAmenity");
            villa.TotalCapacity = _unitOfWork.VillaNumber.GetAll(vn => vn.VillaId == id).Sum(vn => vn.Capacity);
            return villa;
        }

        public IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate, int requested2p = 0, int requested3p = 0)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumbersList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusApproved ||
            u.Status == SD.StatusCheckedIn ||
            u.Status == SD.StatusPending).ToList();

            foreach (var villa in villaList)
            {
                int roomAvailable = SD.VillaRoomsAvailable_Count
                    (villa.Id, villaNumbersList, checkInDate, nights, bookedVillas);

                var villaRooms = villaNumbersList.Where(vn => vn.VillaId == villa.Id).ToList();
                villa.TotalCapacity = villaRooms.Sum(vn => vn.Capacity);
                villa.Rooms2p = villaRooms.Count(vn => vn.Capacity <= 2);
                villa.Rooms3p = villaRooms.Count(vn => vn.Capacity >= 3);
                villa.AvailableRoomsCount = roomAvailable;

                var overlapping = bookedVillas.Where(b => b.VillaId == villa.Id &&
                    b.CheckInDate < checkInDate.AddDays(nights) &&
                    checkInDate < b.CheckOutDate).ToList();

                int taken2p = 0, taken3p = 0;
                foreach (var b in overlapping)
                {
                    if (b.VillaNumber > 0)
                    {
                        var room = villaNumbersList.FirstOrDefault(vn => vn.Villa_Number == b.VillaNumber);
                        if (room != null)
                        {
                            if (room.Capacity <= 2) taken2p++;
                            else taken3p++;
                        }
                    }
                    else
                    {
                        taken2p++;
                    }
                }
                villa.Available2p = Math.Max(0, villa.Rooms2p - taken2p);
                villa.Available3p = Math.Max(0, villa.Rooms3p - taken3p);

                bool lacks2p = requested2p > 0 && villa.Available2p < requested2p;
                bool lacks3p = requested3p > 0 && villa.Available3p < requested3p;

                if (roomAvailable == 0)
                {
                    villa.IsAvailable = false;
                    villa.UnavailableReason = "Sold Out";
                }
                else if (lacks2p && lacks3p)
                {
                    villa.IsAvailable = false;
                    villa.UnavailableReason = "Not enough 2-person or 3-person rooms available";
                }
                else if (lacks2p)
                {
                    villa.IsAvailable = false;
                    villa.UnavailableReason = "Not enough 2-person rooms available";
                }
                else if (lacks3p)
                {
                    villa.IsAvailable = false;
                    villa.UnavailableReason = "Not enough 3-person rooms available";
                }
                else
                {
                    villa.IsAvailable = true;
                    villa.UnavailableReason = null;
                }
            }
            return villaList;
        }

        public bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate)
        {
            var villaNumbersList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusApproved ||
            u.Status == SD.StatusCheckedIn ||
            u.Status == SD.StatusPending).ToList();

            int roomAvailable = SD.VillaRoomsAvailable_Count
                (villaId, villaNumbersList, checkInDate, nights, bookedVillas);

            return roomAvailable > 0;
        }

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var filseStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                villa.Image.CopyTo(filseStream);

                villa.ImageUrl = @"\images\VillaImage\" + fileName;

            }


            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();
        }
    }
}
