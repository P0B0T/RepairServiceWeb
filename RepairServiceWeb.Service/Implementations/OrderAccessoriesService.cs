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

        /// <summary>
        /// Метод для получения списка заказов запчастей
        /// </summary>
        /// <returns>Список заказов запчастей или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetAll()
        {
            try
            {
                var orderAccessories = await _orderAccessoriesRepository.GetAll()
                                                                        .Include(x => x.Client)
                                                                        .Include(x => x.Accessories)
                                                                        .ToListAsync();

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

        /// <summary>
        /// Метод для получения отфильтрованного списка заказов запчастей
        /// </summary>
        /// <param name="clientFullName"> - фио клиента</param>
        /// <param name="accessoryName"> - название запчасти</param>
        /// <param name="count"> - количество запчастей</param>
        /// <param name="date"> - дата заказа</param>
        /// <param name="status"> - статус заказа</param>
        /// <returns>Отфильтрованный список заказов запчастей или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetFiltered(string clientFullName = "", string accessoryName = "", int? count = null, DateOnly date = default, string status = "")
        {
            try
            {
                var orderAccessories = await _orderAccessoriesRepository.GetAll()
                                                                        .Include(x => x.Client)
                                                                        .Include(x => x.Accessories)
                                                                        .ToListAsync();

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

        /// <summary>
        /// Метод для получения отфильтрованного списка заказов запчастей по пользователю
        /// </summary>
        /// <param name="userId"> - код пользователя</param>
        /// <param name="login"> - логин</param>
        /// <param name="password"> - пароль</param>
        /// <returns>Отфильтрованный список заказов запчастей или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetFilteredByUser(int? userId, string login = "", string password = "")
        {
            try
            {
                var clientsOrder = (await _orderAccessoriesRepository.GetAll()
                                                              .Include(x => x.Accessories)
                                                              .Include(x => x.Client)
                                                              .ToListAsync())
                                                              .Where(x => x.Client.Id == userId && x.Client.Login == login && x.Client.Password == password);

                if (!clientsOrder.Any())
                {
                    return new BaseResponse<IEnumerable<OrderAccessory>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<OrderAccessory>>()
                {
                    Data = clientsOrder,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrderAccessory>>()
                {
                    Description = $"[GetFilteredByUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения заказа по коду
        /// </summary>
        /// <param name="id"> - код заказа</param>
        /// <returns>Информация о заказе или сообщение "Элемент не найден"</returns>
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

                // Перенос данных заказа в ViewModel
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

        /// <summary>
        /// Метод для получения заказов по названию запчасти
        /// </summary>
        /// <param name="name"> - название запчасти</param>
        /// <returns>Список найденных заказов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<OrderAccessory>>> GetByName(string name)
        {
            try
            {
                var orderAccessories = (await _orderAccessoriesRepository.GetAll()
                                                                         .Include(x => x.Client)
                                                                         .Include(x => x.Accessories)
                                                                         .ToListAsync())
                                                                         .Where(x => x.Accessories.Name.ToLower().Contains(name.ToLower()));

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
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для удаления заказа
        /// </summary>
        /// <param name="id"> - код заказа</param>
        /// <returns>"Успех" или сообщение "Элемент не найден"</returns>
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

        /// <summary>
        /// Метод для создания заказа
        /// </summary>
        /// <param name="orderAccessoriesViewModel"> - ViewModel</param>
        /// <returns>Добавленный заказ</returns>
        public async Task<IBaseResponse<OrderAccessory>> Create(OrderAccessoriesViewModel orderAccessoriesViewModel)
        {
            try
            {
                // Перенос данных из ViewModel
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

        /// <summary>
        /// Метод для редактирования заказа
        /// </summary>
        /// <param name="id"> - код заказа</param>
        /// <param name="orderAccessoriesViewModel"> - ViewModel</param>
        /// <returns>Изменённый заказ или сообщение "Элемент не найден"</returns>
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

                // Перенос данных из ViewModel
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
