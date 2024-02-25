using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.DAL.Repositories
{
    public class RepairsRepository : IBaseRepository<Repair>
    {
        private readonly ApplicationDbContext _context;

        public RepairsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Repair entity)
        {
            await _context.Repairs.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Repair entity)
        {
            _context.Repairs.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Repair> GetAll()
        {
            return _context.Repairs;
        }

        public async Task<Repair> Update(Repair entity)
        {
            _context.Repairs.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
