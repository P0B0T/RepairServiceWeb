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

        /// <summary>
        /// Метод добавления записи в таблицу "Заказы комплектующих"
        /// </summary>
        /// <param name="entity" - информация о заказе></param>
        public async Task<bool> Create(OrderAccessory entity)
        {
            await _context.OrderAccessories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод удаления записи из таблицы "Заказы комплектующих"
        /// </summary>
        /// <param name="entity" - информация о заказе></param>
        public async Task<bool> Delete(OrderAccessory entity)
        {
            _context.OrderAccessories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Метод для получения всех данных из таблицы "Заказы комплектующих"
        /// </summary>
        public IQueryable<OrderAccessory> GetAll()
        {
            return _context.OrderAccessories;
        }

        /// <summary>
        /// Метод для обновления данных в таблице "Заказы комплектующих"
        /// </summary>
        /// <param name="entity" - информация о заказе></param>
        public async Task<OrderAccessory> Update(OrderAccessory entity)
        {
            _context.OrderAccessories.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
