using Diplom.Domain.Entity;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;

namespace Diplom.Service.Interfaces
{
    public interface IClientsService : IBaseService<Client>
    {
        Task<IBaseResponse<ClientsViewModel>> Get(int id);

        Task<IBaseResponse<Client>> Create(ClientsViewModel model);

        Task<IBaseResponse<Client>> Edit(int id, ClientsViewModel model);

        Task<IBaseResponse<IEnumerable<Client>>> GetFiltered(string fullName = "", string address = "");
    }
}
