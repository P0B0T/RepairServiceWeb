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
    public class StaffRepositoty : IBaseRepository<Staff>
    {
        private readonly ApplicationDbContext _context;

        public StaffRepositoty(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Staff entity)
        {
            await _context.Staff.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Staff entity)
        {
            _context.Staff.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Staff> GetAll()
        {
            return _context.Staff;
        }

        public async Task<Staff> Update(Staff entity)
        {
            _context.Staff.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
