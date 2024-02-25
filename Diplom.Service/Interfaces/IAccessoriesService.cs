using Diplom.Domain.Entity;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;

namespace Diplom.Service.Interfaces
{
    public interface IAccessoriesService : IBaseService<Accessory>
    {
        Task<IBaseResponse<AccessoriesViewModel>> Get(int id);

        Task<IBaseResponse<Accessory>> Create(AccessoriesViewModel model);

        Task<IBaseResponse<Accessory>> Edit(int id, AccessoriesViewModel model);

        Task<IBaseResponse<IEnumerable<Accessory>>> GetFiltered(string name = null, string manufacturer = null, decimal? cost = null, string supplier = null);
    }
}
