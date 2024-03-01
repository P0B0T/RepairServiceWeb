using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;

namespace Diplom.DAL.Repositories
{
    public class AccessoriesRepository : IBaseRepository<Accessory>
    {
        private readonly ApplicationDbContext _context;

        public AccessoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Accessory entity)
        {
            await _context.Accessories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Accessory entity)
        {
            _context.Accessories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Accessory> GetAll()
        {
            return _context.Accessories;
        }

        public async Task<Accessory> Update(Accessory entity)
        {
            _context.Accessories.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
