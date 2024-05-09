using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL.Interfaces;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.Enum;
using RepairServiceWeb.Domain.Response;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class RepairsService : IRepairsService
    {
        private readonly IBaseRepository<Repair> _repairsRepository;

        public RepairsService(IBaseRepository<Repair> repairsRepository)
        {
            _repairsRepository = repairsRepository;
        }

        /// <summary>
        /// Метод для получения списка ремонтов
        /// </summary>
        /// <returns>Список ремонтов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Repair>>> GetAll()
        {
            try
            {
                var repairs = await _repairsRepository.GetAll()
                                                      .Include(x => x.Staff)
                                                      .Include(x => x.Device)
                                                      .Include(x => x.Device.Client)
                                                      .ToListAsync();

                if (!repairs.Any())
                {
                    return new BaseResponse<IEnumerable<Repair>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Data = repairs,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка ремонтов
        /// </summary>
        /// <param name="clientFullName"> - фио клиента</param>
        /// <param name="staffFullName"> - фио сотрудника</param>
        /// <returns>Отфильтрованный список ремонтов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Repair>>> GetFiltered(string clientFullName = "", string staffFullName = "")
        {
            try
            {
                var repairs = await _repairsRepository.GetAll()
                                                      .Include(x => x.Staff)
                                                      .Include(x => x.Device)
                                                      .Include(x => x.Device.Client)
                                                      .ToListAsync();

                if (clientFullName != "")
                    repairs = repairs.Where(x => x.Device.Client.FullName.ToLower().Contains(clientFullName.ToLower()))
                                     .ToList();

                if (staffFullName != "")
                    repairs = repairs.Where(x => x.Staff.FullName.ToLower().Contains(staffFullName.ToLower()))
                                     .ToList();

                if (!repairs.Any())
                {
                    return new BaseResponse<IEnumerable<Repair>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Data = repairs,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Description = $"[GetFiltered] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка ремонтов по пользователю
        /// </summary>
        /// <param name="userId"> - код пользователя</param>
        /// <param name="login"> - логин</param>
        /// <param name="password"> - пароль</param>
        /// <returns>Отфильтрованный список ремонтов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Repair>>> GetFilteredByUser(int? userId, string login = "", string password = "")
        {
            try
            {
                var clientsRepairs = (await _repairsRepository.GetAll()
                                                              .Include(x => x.Staff)
                                                              .Include(x => x.Device)
                                                              .Include(x => x.Device.Client)
                                                              .ToListAsync())
                                                              .Where(x => x.Device.Client.Id == userId && x.Device.Client.Login == login && x.Device.Client.Password == password);

                var staffRepairs = (IQueryable<Repair>)null;

                if (!clientsRepairs.Any())
                {
                    staffRepairs = _repairsRepository.GetAll()
                                                     .Include(x => x.Staff)
                                                     .Include(x => x.Device)
                                                     .Include(x => x.Device.Client)
                                                     .Where(x => x.Staff.Id == userId && x.Staff.Login == login && x.Staff.Password == password);
                }

                if (!clientsRepairs.Any() && !staffRepairs.Any())
                {
                    return new BaseResponse<IEnumerable<Repair>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Data = clientsRepairs.Any() ? clientsRepairs : staffRepairs,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Description = $"[GetFilteredByUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения ремонта по коду
        /// </summary>
        /// <param name="id"> - код ремонта</param>
        /// <returns>Информация о ремонте или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<RepairsViewModel>> Get(int id)
        {
            try
            {
                var repairs = await _repairsRepository.GetAll()
                                                      .Include(x => x.Staff)
                                                      .Include(x => x.Device)
                                                      .Include(x => x.Device.Client)
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (repairs == null)
                {
                    return new BaseResponse<RepairsViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RepairsNotFound
                    };
                }

                // Перенос данных ремонта в ViewModel
                var data = new RepairsViewModel()
                {
                    Id = repairs.Id,
                    DeviceId = repairs.DeviceId,
                    StaffId = repairs.StaffId,
                    Date_of_admission = repairs.DateOfAdmission,
                    End_date = repairs.EndDate,
                    Cost = repairs.Cost,
                    Description_of_problem = repairs.DescriptionOfProblem,
                    Descriprion_of_work_done = repairs.DescriprionOfWorkDone,
                    Staff = repairs.Staff,
                    Device = repairs.Device
                };

                return new BaseResponse<RepairsViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<RepairsViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для получения ремонтов по модели устройства
        /// </summary>
        /// <param name="name"> - модель устройства</param>
        /// <returns>Список найденных ремонтов или сообщение "Элементы не найдены"</returns>
        public async Task<IBaseResponse<IEnumerable<Repair>>> GetByName(string name)
        {
            try
            {
                var repairs = (await _repairsRepository.GetAll()
                                                       .Include(x => x.Staff)
                                                       .Include(x => x.Device)
                                                       .Include(x => x.Device.Client)
                                                       .ToListAsync())
                                                       .Where(x => x.Device.Model.ToLower().Contains(name.ToLower()));

                if (!repairs.Any())
                {
                    return new BaseResponse<IEnumerable<Repair>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Data = repairs,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Repair>>()
                {
                    Description = $"[GetByName] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для удаления ремонта
        /// </summary>
        /// <param name="id"> - код ремонта</param>
        /// <returns>"Успех" или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var repairs = await _repairsRepository.GetAll()
                                                          .FirstOrDefaultAsync(x => x.Id == id);

                if (repairs == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RepairsNotFound,
                        Data = false
                    };
                }

                await _repairsRepository.Delete(repairs);

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
        /// Метод для создания ремонта
        /// </summary>
        /// <param name="repairsViewModel"> - ViewModel</param>
        /// <returns>Добавленный ремонт</returns>
        public async Task<IBaseResponse<Repair>> Create(RepairsViewModel repairsViewModel)
        {
            try
            {
                // Перенос данных из ViewModel
                var repairs = new Repair()
                {
                    Id = repairsViewModel.Id,
                    DeviceId = repairsViewModel.DeviceId,
                    StaffId = repairsViewModel.StaffId,
                    DateOfAdmission = repairsViewModel.Date_of_admission,
                    EndDate = repairsViewModel.End_date,
                    Cost = repairsViewModel.Cost,
                    DescriptionOfProblem = repairsViewModel.Description_of_problem,
                    DescriprionOfWorkDone = repairsViewModel.Descriprion_of_work_done,
                    Staff = repairsViewModel.Staff,
                    Device = repairsViewModel.Device
                };

                await _repairsRepository.Create(repairs);

                return new BaseResponse<Repair>()
                {
                    StatusCode = StatusCode.OK,
                    Data = repairs
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Repair>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Метод для редактирования ремонта
        /// </summary>
        /// <param name="id"> - код ремонта</param>
        /// <param name="repairsViewModel"> - ViewModel</param>
        /// <returns>Изменённый ремонт или сообщение "Элемент не найден"</returns>
        public async Task<IBaseResponse<Repair>> Edit(int id, RepairsViewModel repairsViewModel)
        {
            try
            {
                var repairs = await _repairsRepository.GetAll()
                                                      .FirstOrDefaultAsync(x => x.Id == id);

                if (repairs == null)
                {
                    return new BaseResponse<Repair>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RepairsNotFound
                    };
                }

                // Перенос данных из ViewModel
                repairs.DeviceId = repairsViewModel.DeviceId;
                repairs.StaffId = repairsViewModel.StaffId;
                repairs.DateOfAdmission = repairsViewModel.Date_of_admission;
                repairs.EndDate = repairsViewModel.End_date;
                repairs.Cost = repairsViewModel.Cost;
                repairs.DescriptionOfProblem = repairsViewModel.Description_of_problem;
                repairs.DescriprionOfWorkDone = repairsViewModel.Descriprion_of_work_done;

                await _repairsRepository.Update(repairs);

                return new BaseResponse<Repair>()
                {
                    Data = repairs,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Repair>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
