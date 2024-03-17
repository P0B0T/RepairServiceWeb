using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
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
