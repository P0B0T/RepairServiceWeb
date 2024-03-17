using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IAccessoriesService : IBaseService<Accessory>
    {
        Task<IBaseResponse<AccessoriesViewModel>> Get(int id);

        Task<IBaseResponse<Accessory>> Create(AccessoriesViewModel model);

        Task<IBaseResponse<Accessory>> Edit(int id, AccessoriesViewModel model);

        Task<IBaseResponse<IEnumerable<Accessory>>> GetFiltered(string name = "", string manufacturer = "", string supplier = "");
    }
}
