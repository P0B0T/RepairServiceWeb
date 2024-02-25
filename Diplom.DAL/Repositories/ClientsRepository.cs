using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Diplom.DAL.Repositories
{
    public class ClientsRepository : IBaseRepository<Client>
    {
        private readonly ApplicationDbContext _context;

        public ClientsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Client entity)
        {
            await _context.Clients.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Client entity)
        {
            _context.Clients.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Client> GetAll()
        {
            return _context.Clients;
        }

        public async Task<Client> Update(Client entity)
        {
            _context.Clients.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
