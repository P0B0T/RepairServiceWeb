using Diplom.Domain.Entity;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Diplom.Service.Interfaces
{
    public interface IDevicesService : IBaseService<Device>
    {
        Task<IBaseResponse<DevicesViewModel>> Get(int id);

        Task<IBaseResponse<Device>> Create(DevicesViewModel model, IFormFile? file = null);

        Task<IBaseResponse<Device>> Edit(int id, DevicesViewModel model, IFormFile? file = null);

        Task<IBaseResponse<IEnumerable<Device>>> GetFiltered(string manufacturer = "", string type = "", string clientFullName = "");
    }
}
