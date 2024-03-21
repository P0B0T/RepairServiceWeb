using Microsoft.AspNetCore.Http;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IDevicesService : IBaseService<Device>
    {
        Task<IBaseResponse<DevicesViewModel>> Get(int id);

        Task<IBaseResponse<Device>> Create(DevicesViewModel model, IFormFile? file = null);

        Task<IBaseResponse<Device>> Edit(int id, DevicesViewModel model, IFormFile? file = null);

        Task<IBaseResponse<IEnumerable<Device>>> GetFiltered(string manufacturer = "", string type = "", string clientFullName = "");

        Task<IBaseResponse<IEnumerable<Device>>> GetFilteredByUser(int? userId, string login = "", string password = "");
    }
}
