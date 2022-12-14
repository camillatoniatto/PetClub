using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.ViewModels.Pet;
using PetClub.AppService.ViewModels.Scheduler;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.SchedulerAppService
{
    public class AppServiceScheduler : IAppServiceScheduler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;
        private readonly IAppServicePet _appServicePet;

        public AppServiceScheduler(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier, IAppServicePet appServicePet)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
            _appServicePet = appServicePet;
        }

        public async Task<string> CreateScheduler(CreateSchedulerViewModel model)
        {
            try
            {
                var date = DateTime.MinValue;
                if (model.StartDate == date || model.FinalDate == date)
                {
                    _notifier.Handle(new NotificationMessage("Date", "Datas inválidas"));
                    throw new Exception ("Datas inválidas");
                }
                if (model.StartDate > model.FinalDate)
                {
                    _notifier.Handle(new NotificationMessage("Date", "A data de início não pode ser maior que a data final."));
                    throw new Exception("A data de início não pode ser maior que a data final.");
                }
                await CheckAvailable(model.IdPet, model.StartDate, model.FinalDate, null);
                var serviceType = GetSchedulerServiceTypeInt(model.ServiceType);
                var schedulerSituation = GetSchedulerSituationTypeInt(model.SchedulerSituation);
                
                var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(model.IdPartner));
                var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(model.IdPet));
                if (pet == null)
                {
                    _notifier.Handle(new NotificationMessage("Erro", "Animal não encontrado."));
                    throw new Exception("Animal não encontrado.");
                }
                var scheduler = await _unitOfWork.IRepositoryScheduler.AddReturnIdAsync(new Scheduler(model.IdPartner, model.IdPet, model.StartDate, model.FinalDate,
                                                                                            serviceType, schedulerSituation, DateTime.Now.ToBrasilia()));
                await _unitOfWork.CommitAsync();
                return scheduler;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task CheckAvailable(string idPet, DateTime startDate, DateTime endDate, string idScheduler)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            IList<Scheduler> schedulers = new List<Scheduler>();
            if (!string.IsNullOrEmpty(idScheduler))
            {
                schedulers = await _unitOfWork.IRepositoryScheduler.GetByOrderAsync(x => x.IdPet.Equals(idPet) && x.StartDate >= startDate && x.FinalDate <= endDate
                                                        && (x.SchedulerSituation != SchedulerSituation.CANCELED && x.SchedulerSituation != SchedulerSituation.CONCLUDED) && !x.Id.Equals(idScheduler), x => x.DateCreation, true);

            }
            else
            {
                schedulers = await _unitOfWork.IRepositoryScheduler.GetByOrderAsync(x => x.IdPet.Equals(idPet) && x.StartDate >= startDate && x.FinalDate <= endDate
                                        && (x.SchedulerSituation != SchedulerSituation.CANCELED && x.SchedulerSituation != SchedulerSituation.CONCLUDED), x => x.DateCreation, true);

            }
            if (schedulers.Count() > 0)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Já existe um agendamento em aberto para este animal entre as datas "+ startDate.ToString("d", culture)+ " e "+ endDate.ToString("d", culture)));
                throw new Exception("Já existe um agendamento em aberto para este animal entre as datas " + startDate.ToString("d", culture) + " e " + endDate.ToString("d", culture));
            }
        }

        public async Task<int> CheckQuantitySchedylers(DateTime startDate, DateTime endDate)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var schedulers = await _unitOfWork.IRepositoryScheduler.GetByOrderAsync(x => x.StartDate >= startDate && x.FinalDate <= endDate
                                        && (x.SchedulerSituation != SchedulerSituation.CANCELED && x.SchedulerSituation != SchedulerSituation.CONCLUDED), x => x.DateCreation, true);
            return schedulers.Count();
        }

        public async Task<List<GetSchedulerViewModel>> GetSchedulersByPartner(string idPartner)
        {
            IList<Scheduler> schedulers = new List<Scheduler>();
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idPartner));
            if (user.IsPartner)
            {
                schedulers = await _unitOfWork.IRepositoryScheduler.GetByOrderAsync(x => x.IdPartner.Equals(idPartner), x => x.DateCreation, true);
            }
            else if (user.IsAdmin)
            {
                schedulers = await _unitOfWork.IRepositoryScheduler.GetByOrderAsync(x => x.IdPartner != null, x => x.DateCreation, true);
            }
            else
            {
                var pets = await _unitOfWork.IRepositoryPet.GetByAsync(x => x.IdUser.Equals(idPartner));
                foreach (var pet in pets)
                {
                    var schedulerPet = await _unitOfWork.IRepositoryScheduler.GetByOrderAsync(x => x.IdPet.Equals(pet.Id), x => x.DateCreation, true);
                    foreach (var schedulerPetitem in schedulerPet)
                    {
                        schedulers.Add(schedulerPetitem);
                    }
                }
            }
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetSchedulerViewModel>();
            foreach (var item in schedulers)
            {
                var pet = await _appServicePet.GetPetById(item.IdPet);
                var serviceType = GetSchedulerServiceType(item.ServiceType);
                var schedulerSituation = GetSchedulerSituation(item.SchedulerSituation, item.StartDate);
                var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdPartner));

                list.Add(new GetSchedulerViewModel(item.Id, item.IdPartner, partner.FullName, pet.IdUser, pet.IdPet, pet.Name, item.StartDate, 
                                                   item.FinalDate, item.StartDate.ToString("g", culture), item.FinalDate.ToString("g", culture),
                                                   (int)item.ServiceType, serviceType, (int)item.SchedulerSituation, schedulerSituation,
                                                   item.WriteDate.ToString("d", culture)));

            }
            return list;
        }

        public async Task<List<GetSchedulerPetViewModel>> GetSchedulersByPet(string idPet)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetSchedulerPetViewModel>();
            var schedulers = await _unitOfWork.IRepositoryScheduler.GetByOrderAsync(x => x.IdPet.Equals(idPet), x => x.DateCreation, true);
            foreach (var item in schedulers)
            {
                var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(item.IdPartner));
                var pet = await _appServicePet.GetPetById(item.IdPet);
                var serviceType = GetSchedulerServiceType(item.ServiceType);
                var schedulerSituation = GetSchedulerSituation(item.SchedulerSituation, item.StartDate);
                list.Add(new GetSchedulerPetViewModel(item.Id, item.IdPartner, partner.FullName, pet.IdUser, pet.IdPet, pet.Name, item.StartDate, item.FinalDate,
                                                    item.StartDate.ToString("g", culture), item.FinalDate.ToString("g", culture),
                                                    (int)item.ServiceType, serviceType, (int)item.SchedulerSituation, schedulerSituation, 
                                                    item.WriteDate.ToString("d", culture)));

            }
            return list;
        }

        public async Task<GetSchedulerViewModel> GetSchedulersById(string idScheduler)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetSchedulerViewModel>();
            var scheduler = await _unitOfWork.IRepositoryScheduler.GetByIdAsync(x => x.Id.Equals(idScheduler));
            var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(scheduler.IdPartner));
            var pet = await _appServicePet.GetPetById(scheduler.IdPet);
            var serviceType = GetSchedulerServiceType(scheduler.ServiceType);
            var schedulerSituation = GetSchedulerSituation(scheduler.SchedulerSituation, scheduler.StartDate);
            return new GetSchedulerViewModel(scheduler.Id, scheduler.IdPartner, partner.FullName, pet.IdUser, pet.IdPet, pet.Name, scheduler.StartDate, 
                                    scheduler.FinalDate, scheduler.StartDate.ToString("g", culture), scheduler.FinalDate.ToString("g", culture),
                                    (int)scheduler.ServiceType, serviceType, (int)scheduler.SchedulerSituation, schedulerSituation,
                                    scheduler.WriteDate.ToString("d", culture));
        }

        public async Task UpdateScheduler(UpdateSchedulerViewModel model, string idUser)
        {
            try
            {
                var scheduler = await _unitOfWork.IRepositoryScheduler.GetByIdAsync(x => x.Id.Equals(model.IdScheduler));
                if (scheduler.SchedulerSituation == SchedulerSituation.CANCELED)
                {
                    _notifier.Handle(new NotificationMessage("Erro", "Não é possível editar um agendamento com a situação 'Cancelado'."));
                    throw new Exception("Não é possível editar um agendamento com a situação 'Cancelado'.");
                }

                var date = DateTime.MinValue;
                if (model.StartDate == date || model.FinalDate == date)
                {
                    _notifier.Handle(new NotificationMessage("Date", "Datas inválidas"));
                    throw new Exception("Datas inválidas");
                }
                if (model.StartDate > model.FinalDate)
                {
                    _notifier.Handle(new NotificationMessage("Date", "A data de início não pode ser maior que a data final."));
                    throw new Exception("A data de início não pode ser maior que a data final.");
                }

                await CheckAvailable(model.IdPet, model.StartDate, model.FinalDate, model.IdScheduler);
                var schedulerSituation = GetSchedulerSituationTypeInt(model.SchedulerSituation);
                var serviceType = GetSchedulerServiceTypeInt(model.ServiceType);

                var partner = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idUser));
                var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(model.IdPet));
                if (pet == null)
                {
                    _notifier.Handle(new NotificationMessage("Erro", "Animal não encontrado."));
                    throw new Exception("Animal não encontrado.");
                }
                scheduler.IdPet = model.IdPet;
                scheduler.IdPartner = idUser;
                scheduler.StartDate = model.StartDate;
                scheduler.FinalDate = model.FinalDate;
                scheduler.ServiceType = serviceType;
                scheduler.SchedulerSituation = schedulerSituation;
                scheduler.WriteDate = DateTime.Now.ToBrasilia();
                await _unitOfWork.IRepositoryScheduler.UpdateAsync(scheduler);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task DeleteScheduler(string idScheduler)
        {
            var scheduler = await _unitOfWork.IRepositoryScheduler.GetByIdAsync(x => x.Id.Equals(idScheduler));
            if (scheduler.SchedulerSituation != SchedulerSituation.SCHEDULED)
            {
                _notifier.Handle(new NotificationMessage("Erro", "Só é possível cancelar ou excluir agendamento com a situação 'Agendado'."));
                throw new Exception("Só é possível cancelar ou excluir agendamento com a situação 'Agendado'.");
            }
            scheduler.RecordSituation = RecordSituation.INACTIVE;
            scheduler.SchedulerSituation = SchedulerSituation.CANCELED;
            await _unitOfWork.IRepositoryScheduler.UpdateAsync(scheduler);
            await _unitOfWork.CommitAsync();
        }

        public ServiceType GetSchedulerServiceTypeInt(int serviceType)
        {
            var type = ServiceType.HOST;
            switch (serviceType)
            {
                case 0:
                    type = ServiceType.HOST;
                    break;
                case 1:
                    type = ServiceType.WALK_DOG;
                    break;
                case 2:
                    type = ServiceType.VET_SERVICE;
                    break;
                case 3:
                    type = ServiceType.PET_GROOMING;
                    break;
                case 4:
                    type = ServiceType.OTHER;
                    break;
            }
            return type;
        }

        public SchedulerSituation GetSchedulerSituationTypeInt(int shedulerSituation)
        {
            var type = SchedulerSituation.SCHEDULED;
            switch (shedulerSituation)
            {
                case 0:
                    type = SchedulerSituation.SCHEDULED;
                    break;
                case 1:
                    type = SchedulerSituation.CONCLUDED;
                    break;
                case 2:
                    type = SchedulerSituation.CANCELED;
                    break;
                case 3:
                    type = SchedulerSituation.IN_SERVICE;
                    break;
            }
            return type;
        }

        public string GetSchedulerServiceType(ServiceType serviceType)
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
            }
            return result;
        }

        public string GetSchedulerSituation(SchedulerSituation schedulerSituation, DateTime startDate)
        {
            var result = "";
            switch (schedulerSituation)
            {
                case SchedulerSituation.SCHEDULED:
                    result = "Agendado";
                    break;
                case SchedulerSituation.CONCLUDED:
                    result = "Concluido";
                    break;
                case SchedulerSituation.CANCELED:
                    result = "Cancelado";
                    break;
                case SchedulerSituation.IN_SERVICE:
                    result = "Em Atendimento";
                    break;
            }
            var date = DateTime.Now.ToBrasilia().Date;
            if (result == "Agendado" && date > startDate)
            {
                result = "Em atraso";
            }

            return result;
        }
    }
}
