using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.ViewModels.UsersPartners;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.AppService.ViewModels.Pet;
using System.Globalization;
using PetClub.Domain.Extensions;
using System.Data;

namespace PetClub.AppService.AppServices.PetAppService
{
    public class AppServicePet : IAppServicePet
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServicePet(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }

        public async Task<string> CreatePet(CreatePetViewModel model)
        {
            try
            {
                var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Name.Equals(model.Name) && x.Specie.Equals(model.Specie) && x.Brand.Equals(model.Brand) && x.IdUser.Equals(model.idUser) && x.Birthdate.Date.Equals(model.Birthdate.Date) && x.IsAlive.Equals(true));
                if (pet != null)
                {
                    _notifier.Handle(new NotificationMessage("pet", "Ops, parece que esse animal já foi cadastrado anteriormente."));
                    throw new Exception("Parece que esse animal já foi cadastrado anteriormente.");
                }
                var genre = Genre.MALE;
                if (model.Genre == 1)
                    genre = Genre.FEMALE;

                var idPet = await _unitOfWork.IRepositoryPet.AddReturnIdAsync(new Pet(model.idUser, model.Name, genre, model.Specie, model.Brand, model.Birthdate, true, DateTime.MinValue));
                await _unitOfWork.CommitAsync();
                return idPet;
            }
            catch (Exception e)
            {
                _notifier.Handle(new NotificationMessage("", e.Message));
                return e.Message;
            }
        }

        public async Task<GetPetViewModel> GetPetById(string idPet)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetPetViewModel>();
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = t => t.Include(a => a.User);
            var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(idPet), include);

            var genre = GetGenre(pet.Genre);
            return new GetPetViewModel(pet.Id, pet.IdUser, pet.User.FullName, pet.Name, (int)pet.Genre, genre, pet.Specie, pet.Brand, pet.Birthdate.ToString("d", culture), pet.IsAlive, pet.WriteDate.ToString("d", culture), pet.Birthdate);
        }

        public async Task<List<GetPetViewModel>> GetPetsUser(string idUser, string otherId)
        {
            if (idUser == null || idUser == "undefined")
            {
                idUser = otherId;
            }
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetPetViewModel>();
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = t => t.Include(a => a.User);
            var pets = await _unitOfWork.IRepositoryPet.GetByOrderAsync(x => x.IdUser.Equals(idUser) && x.RecordSituation == RecordSituation.ACTIVE, x => x.Name, false, include);
            foreach (var pet in pets)
            {
                var genre = GetGenre(pet.Genre);
                var item = new GetPetViewModel(pet.Id, idUser, pet.User.FullName, pet.Name, (int)pet.Genre, genre, pet.Specie, pet.Brand, pet.Birthdate.ToString("d", culture), pet.IsAlive, pet.WriteDate.ToString("d", culture), pet.Birthdate);
                list.Add(item);
            }
            return list;
        }

        public async Task<List<GetPetViewModel>> GetAllPets()
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetPetViewModel>();
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = t => t.Include(a => a.User);
            var pets = await _unitOfWork.IRepositoryPet.GetByOrderAsync(x => x.RecordSituation == RecordSituation.ACTIVE, x => x.Name, false, include);
            foreach (var pet in pets)
            {
                var genre = GetGenre(pet.Genre);
                var item = new GetPetViewModel(pet.Id, pet.IdUser, pet.User.FullName, pet.Name, (int)pet.Genre, genre, pet.Specie, pet.Brand, pet.Birthdate.ToString("d", culture), pet.IsAlive, pet.WriteDate.ToString("d", culture), pet.Birthdate);
                list.Add(item);
            }
            return list;
        }

        public async Task<List<GetPetViewModel>> GetAllPetsClient(string idUser)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            var list = new List<GetPetViewModel>();
            var clients = await _unitOfWork.IRepositoryUsersPartners.GetByAsync(x => x.IdPartner.Equals(idUser));
            foreach (var client in clients)
            {
                Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = t => t.Include(a => a.User);
                var pets = await _unitOfWork.IRepositoryPet.GetByOrderAsync(x => x.IdUser.Equals(client.IdUser) && x.RecordSituation == RecordSituation.ACTIVE, x => x.Name, false, include);
                foreach (var pet in pets)
                {
                    var genre = GetGenre(pet.Genre);
                    var item = new GetPetViewModel(pet.Id, pet.IdUser, pet.User.FullName, pet.Name, (int)pet.Genre, genre, pet.Specie, pet.Brand, pet.Birthdate.ToString("d", culture), pet.IsAlive, pet.WriteDate.ToString("d", culture), pet.Birthdate);
                    list.Add(item);
                }
            }
            var myPets = await GetPetsUser(idUser, idUser);
            foreach (var my in myPets)
            {
                list.Add(my);
            }
            return list;
        }

        public async Task UpdatePet(UpdatePetViewModel model)
        {
            var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(model.IdPet));
            if (pet == null)
            {
                _notifier.Handle(new NotificationMessage("pet", "Pet não encontrado."));
                throw new Exception("Pet não encontrado.");
            }

            var genre = Genre.MALE;
            if (model.Genre == 1)
                genre = Genre.FEMALE;


            pet.Name = model.Name;
            pet.Specie = model.Specie;
            pet.Brand = model.Brand;
            pet.Birthdate = model.Birthdate;
            pet.Genre = genre;
            pet.IsAlive = model.IsAlive;
            pet.WriteDate = DateTime.Now.ToBrasilia();
            await _unitOfWork.IRepositoryPet.UpdateAsync(pet);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePet(string idPet)
        {
            var pet = await _unitOfWork.IRepositoryPet.GetByIdAsync(x => x.Id.Equals(idPet));
            if (pet == null)
            {
                _notifier.Handle(new NotificationMessage("pet", "Pet não encontrado."));
                throw new Exception("Pet não encontrado.");
            }

            pet.RecordSituation = RecordSituation.INACTIVE;
            await _unitOfWork.IRepositoryPet.UpdateAsync(pet);
            await _unitOfWork.CommitAsync();
        }

        public string GetGenre(Genre genre)
        {
            var result = "";
            switch (genre)
            {
                case Genre.MALE:
                    result = "Macho";
                    break;
                case Genre.FEMALE:
                    result = "Fêmea";
                    break;
            }
            return result;
        }
    }
}
