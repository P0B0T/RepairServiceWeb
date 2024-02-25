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
    public class OrderAccessoriesRepository : IBaseRepository<OrderAccessory>
    {
        private readonly ApplicationDbContext _context;

        public OrderAccessoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(OrderAccessory entity)
        {
            await _context.OrderAccessories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(OrderAccessory entity)
        {
            _context.OrderAccessories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<OrderAccessory> GetAll()
        {
            return _context.OrderAccessories;
        }

        public async Task<OrderAccessory> Update(OrderAccessory entity)
        {
            _context.OrderAccessories.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
