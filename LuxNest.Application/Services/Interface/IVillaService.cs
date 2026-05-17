using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuxNest.Domain.Entities;

namespace LuxNest.Application.Services.Interface
{
    public interface IVillaService
    {
        IEnumerable<Villa> GetAllVillas();
        Villa GetVillaById(int id);
        void CreateVilla(Villa villa);
        void UpdateVilla(Villa villa);
        bool DeleteVilla(int id);

        IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate, int requested2p = 0, int requested3p = 0);
        bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate);
    }
}
