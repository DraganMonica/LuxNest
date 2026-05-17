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
    public class VillaRepository : Repository<Villa>,IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Villa villa)
        {
            _db.Update(villa);
        }
    }
}
