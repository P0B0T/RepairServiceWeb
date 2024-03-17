using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IOrderAccessoriesService : IBaseService<OrderAccessory>
    {
        Task<IBaseResponse<OrderAccessoriesViewModel>> Get(int id);

        Task<IBaseResponse<OrderAccessory>> Create(OrderAccessoriesViewModel model);

        Task<IBaseResponse<OrderAccessory>> Edit(int id, OrderAccessoriesViewModel model);

        Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetFiltered(string clientFullName = "", string accessoryName = "", int? count = null, DateOnly date = default, string status = "");
    }
}
