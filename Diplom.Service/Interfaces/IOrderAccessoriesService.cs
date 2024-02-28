using Diplom.Domain.Entity;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;

namespace Diplom.Service.Interfaces
{
    public interface IOrderAccessoriesService : IBaseService<OrderAccessory>
    {
        Task<IBaseResponse<OrderAccessoriesViewModel>> Get(int id);

        Task<IBaseResponse<OrderAccessory>> Create(OrderAccessoriesViewModel model);

        Task<IBaseResponse<OrderAccessory>> Edit(int id, OrderAccessoriesViewModel model);

        Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetFiltered(string clientFullName = "", string accessoryName = "", int? count = null, DateOnly date = default, string status = "");
    }
}
