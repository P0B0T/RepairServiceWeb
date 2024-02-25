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
    public class DevicesRepository : IBaseRepository<Device>
    {
        private readonly ApplicationDbContext _context;

        public DevicesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Device entity)
        {
            await _context.Devices.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Device entity)
        {
            _context.Devices.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Device> GetAll()
        {
            return _context.Devices;
        }

        public async Task<Device> Update(Device entity)
        {
            _context.Devices.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
