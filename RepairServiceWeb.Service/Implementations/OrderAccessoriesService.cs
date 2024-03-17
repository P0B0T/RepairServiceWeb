using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Enum;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class OrderAccessoriesService : IOrderAccessoriesService
    {
        private readonly IBaseRepository<OrderAccessory> _orderAccessoriesRepository;

        public OrderAccessoriesService(IBaseRepository<OrderAccessory> orderAccessoriesRepository)
        {
            _orderAccessoriesRepository = orderAccessoriesRepository;
        }

        public async Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetAll()
        {
            try
            {
                var orderAccessories = _orderAccessoriesRepository.GetAll()
                                                                  .Include(x => x.Client)
                                                                  .Include(x => x.Accessories);

                if (!orderAccessories.Any())
                {
                    return new BaseResponse<IEnumerable<OrderAccessory>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<OrderAccessory>>()
                {
                    Data = orderAccessories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderAccessory>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetFiltered(string clientFullName = "", string accessoryName = "", int? count = null, DateOnly date = default, string status = "")
        {
            try
            {
                var orderAccessories = _orderAccessoriesRepository.GetAll()
                                                                  .Include(x => x.Client)
                                                                  .Include(x => x.Accessories)
                                                                  .ToList();

                if (clientFullName != "")
                    orderAccessories = orderAccessories.Where(x => x.Client.FullName.ToLower().Contains(clientFullName.ToLower()))
                                                       .ToList();

                if (accessoryName != "")
                    orderAccessories = orderAccessories.Where(x => x.Accessories.Name.ToLower().Contains(accessoryName.ToLower()))
                                                       .ToList();

                if (count != null)
                    orderAccessories = orderAccessories.Where(x => x.Count.Contains(count.ToString()))
                                                       .ToList();

                if (date != default)
                    orderAccessories = orderAccessories.Where(x => x.DateOrder == date)
                                                       .ToList();

                if (status != "")
                    orderAccessories = orderAccessories.Where(x => x.StatusOrder.ToLower().Contains(status.ToLower()))
                                                       .ToList();

                if (!orderAccessories.Any())
                {
                    return new BaseResponse<IEnumerable<OrderAccessory>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<OrderAccessory>>()
                {
                    Data = orderAccessories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderAccessory>>()
                {
                    Description = $"[GetFiltered] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<OrderAccessoriesViewModel>> Get(int id)
        {
            try
            {
                var orderAccessories = await _orderAccessoriesRepository.GetAll()
                                                                        .Include(x => x.Client)
                                                                        .Include(x => x.Accessories)
                                                                        .FirstOrDefaultAsync(x => x.Id == id);

                if (orderAccessories == null)
                {
                    return new BaseResponse<OrderAccessoriesViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.OrderAccessoriesNotFound
                    };
                }

                var data = new OrderAccessoriesViewModel()
                {
                    Id = orderAccessories.Id,
                    ClientId = orderAccessories.ClientId,
                    AccessoriesId = orderAccessories.Id,
                    Count = orderAccessories.Count,
                    Cost = orderAccessories.Cost,
                    Date_order = orderAccessories.DateOrder,
                    Status_order = orderAccessories.StatusOrder,
                    Client = orderAccessories.Client,
                    Accessories = orderAccessories.Accessories
                };

                return new BaseResponse<OrderAccessoriesViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderAccessoriesViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<OrderAccessory>> GetByName(string name)
        {
            try
            {
                var orderAccessories = await _orderAccessoriesRepository.GetAll()
                                                                        .Include(x => x.Client)
                                                                        .Include(x => x.Accessories)
                                                                        .FirstOrDefaultAsync(x => x.Accessories.Name.ToLower().Contains(name.ToLower()));

                if (orderAccessories == null)
                {
                    return new BaseResponse<OrderAccessory>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.OrderAccessoriesNotFound
                    };
                }

                var data = new OrderAccessory()
                {
                    Id = orderAccessories.Id,
                    ClientId = orderAccessories.ClientId,
                    AccessoriesId = orderAccessories.Id,
                    Count = orderAccessories.Count,
                    Cost = orderAccessories.Cost,
                    DateOrder = orderAccessories.DateOrder,
                    StatusOrder = orderAccessories.StatusOrder,
                    Client = orderAccessories.Client,
                    Accessories = orderAccessories.Accessories
                };

                return new BaseResponse<OrderAccessory>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderAccessory>()
                {
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var orderAccessories = await _orderAccessoriesRepository.GetAll()
                                                                        .FirstOrDefaultAsync(x => x.Id == id);

                if (orderAccessories == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.OrderAccessoriesNotFound,
                        Data = false
                    };
                }

                await _orderAccessoriesRepository.Delete(orderAccessories);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[Delete] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<OrderAccessory>> Create(OrderAccessoriesViewModel orderAccessoriesViewModel)
        {
            try
            {
                var orderAccessories = new OrderAccessory()
                {
                    ClientId = orderAccessoriesViewModel.ClientId,
                    AccessoriesId = orderAccessoriesViewModel.AccessoriesId,
                    Count = orderAccessoriesViewModel.Count,
                    Cost = orderAccessoriesViewModel.Cost,
                    DateOrder = orderAccessoriesViewModel.Date_order,
                    StatusOrder = orderAccessoriesViewModel.Status_order,
                    Accessories = orderAccessoriesViewModel.Accessories,
                    Client = orderAccessoriesViewModel.Client
                };

                await _orderAccessoriesRepository.Create(orderAccessories);

                return new BaseResponse<OrderAccessory>()
                {
                    StatusCode = StatusCode.OK,
                    Data = orderAccessories
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderAccessory>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<OrderAccessory>> Edit(int id, OrderAccessoriesViewModel orderAccessoriesViewModel)
        {
            try
            {
                var orderAccessories = await _orderAccessoriesRepository.GetAll()
                                                                        .FirstOrDefaultAsync(x => x.Id == id);

                if (orderAccessories == null)
                {
                    return new BaseResponse<OrderAccessory>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.OrderAccessoriesNotFound
                    };
                }

                orderAccessories.ClientId = orderAccessoriesViewModel.ClientId;
                orderAccessories.AccessoriesId = orderAccessoriesViewModel.AccessoriesId;
                orderAccessories.Count = orderAccessoriesViewModel.Count;
                orderAccessories.Cost = orderAccessoriesViewModel.Cost;
                orderAccessories.DateOrder = orderAccessoriesViewModel.Date_order;
                orderAccessories.StatusOrder = orderAccessoriesViewModel.Status_order;

                await _orderAccessoriesRepository.Update(orderAccessories);

                return new BaseResponse<OrderAccessory>()
                {
                    Data = orderAccessories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderAccessory>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
