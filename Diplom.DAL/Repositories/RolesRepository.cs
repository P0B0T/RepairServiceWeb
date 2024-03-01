using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;

namespace Diplom.DAL.Repositories
{
    public class RolesRepository : IBaseRepository<Role>
    {
        private readonly ApplicationDbContext _context;

        public RolesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Role entity)
        {
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Role> GetAll()
        {
            return _context.Roles;
        }

        public async Task<Role> Update(Role entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
