using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Enum;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class ClientsService : IClientsService
    {
        private readonly IBaseRepository<Client> _clientsRepository;

        public ClientsService(IBaseRepository<Client> clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        /// <summary>
        /// Метод для получения списка клиентов
        /// </summary>
        /// <returns>Список клиентов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Client>>> GetAll()
        {
            try
            {
                var clients = await _clientsRepository.GetAll()
                                                      .Include(x => x.Role)
                                                      .ToListAsync();

                if (!clients.Any())
                {
                    return new BaseResponse<IEnumerable<Client>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Client>>()
                {
                    Data = clients,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Client>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка клиентов
        /// </summary>
        /// <param name="fullName"> - фио клиента</param>
        /// <param name="address"> - адрес клиента</param>
        /// <returns>Отфильтрованный список клиентов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Client>>> GetFiltered(string fullName = "", string address = "")
        {
            try
            {
                var clients = await _clientsRepository.GetAll()
                                                      .Include(x => x.Role)
                                                      .ToListAsync();

                if (fullName != "")
                    clients = clients.Where(x => x.FullName.ToLower().Contains(fullName.ToLower()))
                                     .ToList();

                if (address != "")
                    clients = clients.Where(x => x.Address.ToLower().Contains(address.ToLower()))
                                     .ToList();

                if (!clients.Any())
                {
                    return new BaseResponse<IEnumerable<Client>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Client>>()
                {
                    Data = clients,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Client>>()
                {
                    Description = $"[GetFiltered] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения клиента по коду
        /// </summary>
        /// <param name="id"> - код клиента</param>
        /// <returns>Информация о клиенте или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<ClientsViewModel>> Get(int id)
        {
            try
            {
                var clients = await _clientsRepository.GetAll()
                                                      .Include(x => x.Role)
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (clients == null)
                {
                    return new BaseResponse<ClientsViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.ClientsNotFound
                    };
                }

                // Перенос данных клиента в ViewModel
                var data = new ClientsViewModel()
                {
                    Id = clients.Id,
                    Name = clients.Name,
                    Surname = clients.Surname,
                    Patronymic = clients.Patronymic,
                    Address = clients.Address,
                    Phone_number = clients.PhoneNumber,
                    Email = clients.Email,
                    RoleId = clients.RoleId,
                    Login = clients.Login,
                    Password = clients.Password,
                    Role = clients.Role
                };

                return new BaseResponse<ClientsViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClientsViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения клиентов по фио
        /// </summary>
        /// <param name="name"> - фио клиента</param>
        /// <returns>Список найденных клиентов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Client>>> GetByName(string name)
        {
            try
            {
                var clients = (await _clientsRepository.GetAll()
                                                       .Include(x => x.Role)
                                                       .ToListAsync())
                                                       .Where(x => x.FullName.ToLower().Contains(name.ToLower()));

                if (!clients.Any())
                {
                    return new BaseResponse<IEnumerable<Client>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Client>>()
                {
                    Data = clients,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Client>>()
                {
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для удаления клиента
        /// </summary>
        /// <param name="id"> - код клиента</param>
        /// <returns>"Успех" или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var clients = await _clientsRepository.GetAll()
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (clients == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.AccessoriesNotFound,
                        Data = false
                    };
                }

                await _clientsRepository.Delete(clients);

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
        /// Метод для создания клиента
        /// </summary>
        /// <param name="clientsViewModel"> - ViewModel</param>
        /// <returns>Добавленный клиент</returns>
        public async Task<IBaseResponse<Client>> Create(ClientsViewModel clientsViewModel)
        {
            try
            {
                // Перенос данных из ViewModel
                var clients = new Client()
                {
                    Name = clientsViewModel.Name,
                    Surname = clientsViewModel.Surname,
                    Patronymic = clientsViewModel.Patronymic,
                    Address = clientsViewModel.Address,
                    PhoneNumber = clientsViewModel.Phone_number,
                    Email = clientsViewModel.Email,
                    RoleId = clientsViewModel.RoleId,
                    Login = clientsViewModel.Login,
                    Password = clientsViewModel.Password,
                    Role = clientsViewModel.Role
                };

                await _clientsRepository.Create(clients);

                return new BaseResponse<Client>()
                {
                    StatusCode = StatusCode.OK,
                    Data = clients
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Client>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для редактирования клиента
        /// </summary>
        /// <param name="id"> - код клиента</param>
        /// <param name="clientsViewModel"> - ViewModel</param>
        /// <returns>Изменённый клиент или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<Client>> Edit(int id, ClientsViewModel clientsViewModel)
        {
            try
            {
                var clients = await _clientsRepository.GetAll()
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (clients == null)
                {
                    return new BaseResponse<Client>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.ClientsNotFound
                    };
                }

                // Перенос данных из ViewModel
                clients.Name = clientsViewModel.Name;
                clients.Surname = clientsViewModel.Surname;
                clients.Patronymic = clientsViewModel.Patronymic;
                clients.Address = clientsViewModel.Address;
                clients.PhoneNumber = clientsViewModel.Phone_number;
                clients.Email = clientsViewModel.Email;
                clients.RoleId = clientsViewModel.RoleId;
                clients.Login = clientsViewModel.Login;
                clients.Password = clientsViewModel.Password;

                await _clientsRepository.Update(clients);

                return new BaseResponse<Client>()
                {
                    Data = clients,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Client>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
