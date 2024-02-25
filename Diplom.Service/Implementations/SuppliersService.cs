using Diplom.DAL.Interfaces;
using Diplom.Domain.Entity;
using Diplom.Domain.Enum;
using Diplom.Domain.Response;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Service.Implementations
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IBaseRepository<Supplier> _suppliersRepository;

        public SuppliersService(IBaseRepository<Supplier> suppliersRepository)
        {
            _suppliersRepository = suppliersRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Supplier>>> GetAll()
        {
            try
            {
                var suppliers = _suppliersRepository.GetAll();

                if (!suppliers.Any())
                {
                    return new BaseResponse<IEnumerable<Supplier>>()
                    {
                        Description = "Элементы не найдены",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<IEnumerable<Supplier>>()
                {
                    Data = suppliers,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Supplier>>()
                {
                    Description = $"[GetAll] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<SuppliersViewModel>> Get(int id)
        {
            try
            {
                var suppliers = await _suppliersRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (suppliers == null)
                {
                    return new BaseResponse<SuppliersViewModel>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.SuppliersNotFound
                    };
                }

                var data = new SuppliersViewModel()
                {
                    Id = suppliers.Id,
                    Company_name = suppliers.CompanyName,
                    Contact_person = suppliers.ContactPerson,
                    Phone_number = suppliers.PhoneNumber,
                    Address = suppliers.Address,
                    Email = suppliers.Email
                };

                return new BaseResponse<SuppliersViewModel>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<SuppliersViewModel>()
                {
                    Description = $"[Get] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Supplier>> GetByName(string name)
        {
            try
            {
                var suppliers = await _suppliersRepository.GetAll().FirstOrDefaultAsync(x => x.CompanyName == name);

                if (suppliers == null)
                {
                    return new BaseResponse<Supplier>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.SuppliersNotFound
                    };
                }

                var data = new Supplier()
                {
                    Id = suppliers.Id,
                    CompanyName = suppliers.CompanyName,
                    ContactPerson = suppliers.ContactPerson,
                    PhoneNumber = suppliers.PhoneNumber,
                    Address = suppliers.Address,
                    Email = suppliers.Email
                };

                return new BaseResponse<Supplier>()
                {
                    StatusCode = StatusCode.OK,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Supplier>()
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
                var suppliers = await _suppliersRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (suppliers == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.SuppliersNotFound,
                        Data = false
                    };
                }

                await _suppliersRepository.Delete(suppliers);

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

        public async Task<IBaseResponse<Supplier>> Create(SuppliersViewModel suppliersViewModel)
        {
            try
            {
                var suppliers = new Supplier()
                {
                    CompanyName = suppliersViewModel.Company_name,
                    ContactPerson = suppliersViewModel.Contact_person,
                    PhoneNumber = suppliersViewModel.Phone_number,
                    Address = suppliersViewModel.Address,
                    Email = suppliersViewModel.Email
                };

                await _suppliersRepository.Create(suppliers);

                return new BaseResponse<Supplier>()
                {
                    StatusCode = StatusCode.OK,
                    Data = suppliers
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Supplier>()
                {
                    Description = $"[Create] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Supplier>> Edit(int id, SuppliersViewModel suppliersViewModel)
        {
            try
            {
                var suppliers = await _suppliersRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

                if (suppliers == null)
                {
                    return new BaseResponse<Supplier>()
                    {
                        Description = "Элемент не найден",
                        StatusCode = StatusCode.SuppliersNotFound
                    };
                }

                suppliers.CompanyName = suppliersViewModel.Company_name;
                suppliers.ContactPerson = suppliersViewModel.Contact_person;
                suppliers.PhoneNumber = suppliersViewModel.Phone_number;
                suppliers.Address = suppliersViewModel.Address;
                suppliers.Email = suppliersViewModel.Email;

                await _suppliersRepository.Update(suppliers);

                return new BaseResponse<Supplier>()
                {
                    Data = suppliers,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Supplier>()
                {
                    Description = $"[Edit] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
