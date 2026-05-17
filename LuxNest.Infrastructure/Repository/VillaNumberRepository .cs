using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LuxNest.Application.Common.Interfaces;
using LuxNest.Domain.Entities;
using LuxNest.Infrastructure.Data;

namespace LuxNest.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(VillaNumber villaNumber)
        {
            _db.Update(villaNumber);
        }
    }
}
