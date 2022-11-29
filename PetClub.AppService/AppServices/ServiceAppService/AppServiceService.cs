using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.ViewModels.Pet;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.AppService.ViewModels.Service;

namespace PetClub.AppService.AppServices.ServiceAppService
{
    public class AppServiceService : IAppServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServiceService(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }

        public async Task<string> CreateService(CreateServiceViewModel model, string idUser)
        {
            try
            {
                var service = await _unitOfWork.IRepositoryService.GetByIdAsync(x => x.Title.Equals(model.Title) && x.IdPartner.Equals(idUser) && x.RecordSituation.Equals(RecordSituation.ACTIVE));
                if (service != null)
                {
                    _notifier.Handle(new NotificationMessage("service", "Já existe um serviço cadastrado com esse título."));
                    throw new Exception("Já existe um serviço cadastrado com esse título.");
                }
                var serviceType = IntToEnumServiceType(model.ServiceType);
                var idService = await _unitOfWork.IRepositoryService.AddReturnIdAsync(new Service(idUser, model.Title, model.Description, serviceType, model.SingleUse, model.DateDuration, model.Value, DateTime.MinValue));
                await _unitOfWork.CommitAsync();
                return idService;
            }
            catch (Exception e)
            {
                _notifier.Handle(new NotificationMessage("", e.Message));
                return e.Message;
            }
        }

        public async Task<GetServiceViewModel> GetServiceById(string idService)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetPetViewModel>();
            var service = await _unitOfWork.IRepositoryService.GetByIdAsync(x => x.Id.Equals(idService));

            var serviceType = GetServiceType(service.ServiceType);
            var purchaseServices = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByAsync(x => x.IdService.Equals(service.Id) && x.RecordSituation.Equals(RecordSituation.ACTIVE));
            var sold = 0;
            foreach (var itemOrder in purchaseServices)
            {
                sold += itemOrder.Quantity;

            }
            return new GetServiceViewModel(service.Id, service.IdPartner, service.Title, service.Description, serviceType, (int)service.ServiceType, service.SingleUser, service.DateDuration.ToString("d", culture), service.Value.ToString("F2"), service.Value, service.WriteDate.ToString("d", culture), service.User.FullName, sold);
        }

        public async Task<List<GetServiceViewModel>> GetServices(bool log, string idUser)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetServiceViewModel>();
            Func<IQueryable<Service>, IIncludableQueryable<Service, object>> include = t => t.Include(a => a.User);
            IList<Service> services = new List<Service>();
            if (log)
            {
                services = await _unitOfWork.IRepositoryService.GetByOrderAsync(x => x.IdPartner.Equals(idUser) && x.RecordSituation == RecordSituation.INACTIVE, x => x.Title, false, include);

            }
            else
            {
                services = await _unitOfWork.IRepositoryService.GetByOrderAsync(x => x.IdPartner.Equals(idUser) && x.RecordSituation == RecordSituation.ACTIVE, x => x.Title, false, include);
            }
            foreach (var service in services)
            {
                var serviceType = GetServiceType(service.ServiceType);
                var purchaseServices = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByAsync(x => x.IdService.Equals(service.Id) && x.RecordSituation.Equals(RecordSituation.ACTIVE));
                var sold = 0;
                foreach (var itemOrder in purchaseServices)
                {
                    sold += itemOrder.Quantity;

                }
                var item = new GetServiceViewModel(service.Id, service.IdPartner, service.Title, service.Description, serviceType, (int)service.ServiceType, service.SingleUser, service.DateDuration.ToString("d", culture), service.Value.ToString("F2"), service.Value, service.WriteDate.ToString("d", culture), service.User.FullName, sold);
                list.Add(item);
            }
            return list;
        }

        public async Task<List<GetServiceViewModel>> GetServicesAdmin()
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetServiceViewModel>();
            Func<IQueryable<Service>, IIncludableQueryable<Service, object>> include = t => t.Include(a => a.User);
            var services = await _unitOfWork.IRepositoryService.GetByOrderAsync(x => x.RecordSituation == RecordSituation.ACTIVE, x => x.Title, false, include);
            foreach (var service in services)
            {
                var serviceType = GetServiceType(service.ServiceType);
                var purchaseServices = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByAsync(x => x.IdService.Equals(service.Id) && x.RecordSituation.Equals(RecordSituation.ACTIVE));
                var sold = 0;
                foreach (var itemOrder in purchaseServices)
                {
                    sold += itemOrder.Quantity;

                }
                var item = new GetServiceViewModel(service.Id, service.IdPartner, service.Title, service.Description, serviceType, (int)service.ServiceType, service.SingleUser, service.DateDuration.ToString("d", culture), service.Value.ToString("F2"), service.Value, service.WriteDate.ToString("d", culture), service.User.FullName, sold);


                list.Add(item);
            }
            return list;
        }

        public async Task<List<GetServiceViewModel>> GetServiceUser(string idUser)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetServiceViewModel>();
            Func<IQueryable<Service>, IIncludableQueryable<Service, object>> include = t => t.Include(a => a.User);
            var services = await _unitOfWork.IRepositoryService.GetByOrderAsync(x => x.IdPartner.Equals(idUser) && x.RecordSituation == RecordSituation.ACTIVE, x => x.Title, false, include);
            foreach (var service in services)
            {
                var serviceType = GetServiceType(service.ServiceType);
                var purchaseServices = await _unitOfWork.IRepositoryPurchaseOrderItem.GetByAsync(x => x.IdService.Equals(service.Id) && x.RecordSituation.Equals(RecordSituation.ACTIVE));
                var sold = 0;
                foreach (var itemOrder in purchaseServices)
                {
                    sold += itemOrder.Quantity;

                }
                var item = new GetServiceViewModel(service.Id, service.IdPartner, service.Title, service.Description, serviceType, (int)service.ServiceType, service.SingleUser, service.DateDuration.ToString("d", culture), service.Value.ToString("F2"), service.Value, service.WriteDate.ToString("d", culture), service.User.FullName, sold);

                list.Add(item);
            }
            return list;
        }

        /*public async Task<List<GetServiceViewModel>> GetAllServices()
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetServiceViewModel>();
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = t => t.Include(a => a.User);
            var services = await _unitOfWork.IRepositoryService.GetByOrderAsync(x => x.RecordSituation == RecordSituation.ACTIVE, x => x.Title, false);
            foreach (var service in services)
            {
                var serviceType = GetServiceType(service.ServiceType);
                var item = new GetServiceViewModel(service.Id, service.IdPartner, service.Title, service.Description, serviceType, service.SingleUser, service.DateDuration.ToString("d", culture), service.Value.ToString("F2"), service.WriteDate.ToString("d", culture));
                list.Add(item);
            }
            return list;
        }*/

        public async Task UpdateService(UpdateServiceViewModel model)
        {
            var service = await _unitOfWork.IRepositoryService.GetByIdAsync(x => x.Id.Equals(model.IdService));
            if (service == null)
            {
                _notifier.Handle(new NotificationMessage("pet", "Registro não encontrado."));
                throw new Exception("Registro não encontrado.");
            }

            service.Title = model.Title;
            service.Description = model.Description;
            service.ServiceType = model.ServiceType;
            service.SingleUser = model.SingleUse;
            service.DateDuration = model.DateDuration;
            service.Value = model.Value;
            service.WriteDate = DateTime.Now.ToBrasilia();
            await _unitOfWork.IRepositoryService.UpdateAsync(service);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteService(string idService)
        {
            var service = await _unitOfWork.IRepositoryService.GetByIdAsync(x => x.Id.Equals(idService));
            if (service == null)
            {
                _notifier.Handle(new NotificationMessage("pet", "Registro não encontrado."));
                throw new Exception("Registro não encontrado.");
            }

            service.RecordSituation = RecordSituation.INACTIVE;
            service.WriteDate = DateTime.Now.ToBrasilia();
            await _unitOfWork.IRepositoryService.UpdateAsync(service);
            await _unitOfWork.CommitAsync();
        }

        public string GetServiceType(ServiceType serviceType)
        {
            var result = "";
            switch (serviceType)
            {
                case ServiceType.HOST:
                    result = "Hospedagem";
                    break;
                case ServiceType.WALK_DOG:
                    result = "Passeio";
                    break;
                case ServiceType.VET_SERVICE:
                    result = "Serviço Veterinário";
                    break;
                case ServiceType.PET_GROOMING:
                    result = "Banho e Tosa";
                    break;
                case ServiceType.OTHER:
                    result = "Outros";
                    break;
                default:
                    break;
            }
            return result;
        }

        public ServiceType IntToEnumServiceType(int value)
        {
            var result = ServiceType.HOST;
            switch (value)
            {
                case 0:
                    result = ServiceType.HOST;
                    break;
                case 1:
                    result = ServiceType.WALK_DOG;
                    break;
                case 2:
                    result = ServiceType.VET_SERVICE;
                    break;
                case 3:
                    result = ServiceType.PET_GROOMING;
                    break;
                case 4:
                    result = ServiceType.OTHER;
                    break;
            }
            return result;
        }
    }
}
