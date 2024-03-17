using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.DAL.Repositories
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
