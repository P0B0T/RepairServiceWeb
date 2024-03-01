using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;

namespace Diplom.DAL.Repositories
{
    public class SuppliersRepository : IBaseRepository<Supplier>
    {
        private readonly ApplicationDbContext _context;

        public SuppliersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Supplier entity)
        {
            await _context.Suppliers.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Supplier entity)
        {
            _context.Suppliers.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Supplier> GetAll()
        {
            return _context.Suppliers;
        }

        public async Task<Supplier> Update(Supplier entity)
        {
            _context.Suppliers.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
