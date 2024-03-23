using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RepairServiceWeb.Service.Interfaces
{
    public interface IRoleCheckerService
    {
        Task<StatusCodeResult> Check(HttpRequest request, string role, string roleAlt);
    }
}
