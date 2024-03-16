using Diplom.DAL.Interfaces;
using Diplom.DAL.Repositories;
using Diplom.Domain.Entity;
using Diplom.Domain.Enum;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Service.Implementations
{
    public class RepairsService : IRepairsService
    {
        private readonly IBaseRepository<Repair> _repairsRepository;

        public RepairsService(IBaseRepository<Repair> repairsRepository)
        {
            _repairsRepository = repairsRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Repair>>> GetAll()
        {
            try
            {
                var repairs = _repairsRepository.GetAll()
                                                .Include(x => x.Staff)
                                                .Include(x => x.Device)
                                                .Include(x => x.Device.Client);

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

        public async Task<IBaseResponse<IEnumerable<Repair>>> GetFiltered(string clientFullName = "", string staffFullName = "")
        {
            try
            {
                var repairs = _repairsRepository.GetAll()
                                                .Include(x => x.Staff)
                                                .Include(x => x.Device)
                                                .Include(x => x.Device.Client)
                                                .ToList();

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

        public async Task<IBaseResponse<Repair>> GetByName(string name)
        {
            try
            {
                var repairs = await _repairsRepository.GetAll()
                                                      .Include(x => x.Staff)
                                                      .Include(x => x.Device)
                                                      .Include(x => x.Device.Client)
                                                      .FirstOrDefaultAsync(x => x.Device.Model.ToLower().Contains(name.ToLower()));

                if (repairs == null)
                {
                    return new BaseResponse<Repair>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.RepairsNotFound
                    };
                }

                var data = new Repair()
                {
                    Id = repairs.Id,
                    DeviceId = repairs.DeviceId,
                    StaffId = repairs.StaffId,
                    DateOfAdmission = repairs.DateOfAdmission,
                    EndDate = repairs.EndDate,
                    Cost = repairs.Cost,
                    DescriptionOfProblem = repairs.DescriptionOfProblem,
                    DescriprionOfWorkDone = repairs.DescriprionOfWorkDone,
                    Staff = repairs.Staff,
                    Device = repairs.Device
                };

                return new BaseResponse<Repair>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Repair>()
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

        public async Task<IBaseResponse<Repair>> Create(RepairsViewModel repairsViewModel)
        {
            try
            {
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
