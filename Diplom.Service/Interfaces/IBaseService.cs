using Diplom.Domain.Entity;
using Diplom.Domain.Response;

namespace Diplom.Service.Interfaces
{
    public interface IBaseService<T>
    {
        Task<IBaseResponse<IEnumerable<T>>> GetAll();

        Task<IBaseResponse<T>> GetByName(string name);

        Task<IBaseResponse<bool>> Delete(int id);
    }
}
