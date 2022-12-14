using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.Image.Model;
using PetClub.AppService.ViewModels.Account;
using PetClub.AppService.ViewModels.Pet;
using PetClub.AppService.ViewModels.User;
using PetClub.Domain.Entities;
using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using PetClub.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.UserAppService
{
    public class AppServiceUser : IAppServiceUser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;
        private readonly IAppServicePet _appServicePet;

        public AppServiceUser(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier, IAppServicePet appServicePet)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
            _appServicePet = appServicePet;
        }

        public async Task<HomeViewModel> GetHomeDetails(string idUser)
        {
            var now = DateTime.Now.ToBrasilia();
            var agendamentos = 0;
            var animaisCadastrados = 0;
            var aniversarios = 0;
            List<AniversarioPetViewModel> petAniversarios = new();
            CultureInfo culture = new CultureInfo("pt-BR");
            Func<IQueryable<Pet>, IIncludableQueryable<Pet, object>> include = t => t.Include(a => a.User);

            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idUser));
            if (user != null)
            {
                if (idUser != null && user.IsAdmin)
                {
                    var scheduler = await _unitOfWork.IRepositoryScheduler.GetByAsync(x => x.StartDate <= now && x.FinalDate >= now && x.RecordSituation.Equals(RecordSituation.ACTIVE));
                    var pets = await _unitOfWork.IRepositoryPet.GetByAsync(x => x.RecordSituation.Equals(RecordSituation.ACTIVE), include);

                    agendamentos = scheduler.Count();
                    animaisCadastrados = pets.Count();
                    aniversarios = pets.Where(x => x.Birthdate.Month.Equals(now.Month) && x.IsAlive).Count();
                    foreach (var pet in pets.Where(x => x.Birthdate.Month.Equals(now.Month) && x.IsAlive))
                    {
                        var genre = GetGenre(pet.Genre);
                        petAniversarios.Add(new AniversarioPetViewModel(pet.Id, pet.User.FullName, pet.Name, genre, pet.Specie, pet.Brand, pet.Birthdate.ToString("d", culture)));
                    }
                }
                else if (idUser != null && user.IsPartner)
                {
                    var scheduler = await _unitOfWork.IRepositoryScheduler.GetByAsync(x => x.StartDate <= now && x.FinalDate >= now && x.IdPartner.Equals(idUser) && x.RecordSituation.Equals(RecordSituation.ACTIVE));
                    var pets = await _appServicePet.GetAllPetsClient(idUser);
                    var petAniversario = pets.Where(x => x.BirthdateDate.Month.Equals(now.Month) && x.IsAlive);

                    agendamentos = scheduler.Count();
                    animaisCadastrados = pets.Count();
                    aniversarios = petAniversario.Count();
                    foreach (var pet in petAniversario)
                    {
                        petAniversarios.Add(new AniversarioPetViewModel(pet.IdPet, pet.Tutor, pet.Name, pet.GenreString, pet.Specie, pet.Brand, pet.Birthdate));
                    }
                }
                else if (idUser != null && !user.IsPartner && !user.IsAdmin)
                {
                    var pets = await _unitOfWork.IRepositoryPet.GetByAsync(x => x.IdUser.Equals(idUser) && x.RecordSituation.Equals(RecordSituation.ACTIVE), include);
                    foreach (var pet in pets)
                    {
                        var scheduler = await _unitOfWork.IRepositoryScheduler.GetByAsync(x => x.StartDate <= now && x.FinalDate >= now && x.IdPet.Equals(pet.Id) && x.RecordSituation.Equals(RecordSituation.ACTIVE));
                        agendamentos += scheduler.Count();
                    }
                    animaisCadastrados = pets.Count();
                    aniversarios = pets.Where(x => x.Birthdate.Month.Equals(now.Month) && x.IsAlive).Count();
                    foreach (var pet in pets.Where(x => x.Birthdate.Month.Equals(now.Month) && x.IsAlive))
                    {
                        var genre = GetGenre(pet.Genre);
                        petAniversarios.Add(new AniversarioPetViewModel(pet.Id, pet.User.FullName, pet.Name, genre, pet.Specie, pet.Brand, pet.Birthdate.ToString("d", culture)));
                    }
                }
            }
            return new HomeViewModel(agendamentos, animaisCadastrados, aniversarios, petAniversarios.OrderBy(x => x.Birthdate).ToList());
        }
        public async Task<UserUpdateViewModel> UpdateAsync(UpdatePerfilUserViewModel updatePerfilUserView)
        {
            try
            {
                string urlImage = "PetClub";
                var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(updatePerfilUserView.Id));
                var date = DateTime.MinValue;

                user.FullName = updatePerfilUserView.FullName != null ? updatePerfilUserView.FullName : user.FullName;
                user.Email = updatePerfilUserView.Email != null ? updatePerfilUserView.Email : user.Email;
                user.Birthdate = updatePerfilUserView.Birthdate != date ? updatePerfilUserView.Birthdate : user.Birthdate;
                user.PhoneNumber = updatePerfilUserView.PhoneNumber != null ? updatePerfilUserView.PhoneNumber : user.PhoneNumber;
                user.AddressName = updatePerfilUserView.AddressName != null ? updatePerfilUserView.AddressName : user.AddressName;
                user.Number = updatePerfilUserView.Number != null ? updatePerfilUserView.Number : user.Number;
                user.Complement = updatePerfilUserView.Complement != null ? updatePerfilUserView.Complement : user.Complement;
                user.Neighborhood = updatePerfilUserView.Neighborhood != null ? updatePerfilUserView.Neighborhood : user.Neighborhood;
                user.City = updatePerfilUserView.City != null ? updatePerfilUserView.City : user.City;
                user.State = updatePerfilUserView.State != null ? updatePerfilUserView.State : user.State;
                user.ZipCode = updatePerfilUserView.ZipCode != null ? updatePerfilUserView.ZipCode : user.ZipCode;
                var result = await _unitOfWork.IRepositoryUser.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                UserUpdateViewModel upt = new UserUpdateViewModel
                {
                    Name = result.FullName,
                    Email = result.Email,
                    Birthdate = result.Birthdate,
                    Image = result.Image,
                    PhoneNumber = result.PhoneNumber,
                };
                return upt;
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        public async Task DeleteUser(string Id)
        {
            CultureInfo culture = new CultureInfo("pt-BR");

            var userById = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(Id));
            userById.Email = "";
            userById.Birthdate = DateTime.MinValue;
            userById.FullName = "";
            userById.PhoneNumber = "";
            userById.IsAdmin = false;
            userById.IsPartner = false;
            userById.IsActive = false;
            userById.AddressName = "";
            userById.Number = "";
            userById.Complement = "";
            userById.Neighborhood = "";
            userById.City = "";
            userById.State = "";
            userById.ZipCode = "";
            userById.Image = "";
            userById.WriteDate = DateTime.Now.ToBrasilia();
            await _unitOfWork.IRepositoryUser.UpdateAsync(userById);
            await _unitOfWork.CommitAsync();
        }

        public async Task<GetUserByIdViewModel> GetByIdAsync(string Id)
        {
            CultureInfo culture = new CultureInfo("pt-BR");

            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(Id));
            var pets = await _appServicePet.GetPetsUser(Id, Id);
            var roles = GetRoles(user);

            return new GetUserByIdViewModel(user.Id, user.FullName, user.Cpf, user.Email, user.PhoneNumber, user.Birthdate.ToString("d", culture),
                user.Image, user.IsAdmin, user.IsPartner, user.AddressName, user.Number, user.Complement, user.Neighborhood,
                user.City, user.State, user.ZipCode, pets, user.IsActive, roles, user.WriteDate, pets.Count());
        }

        public async Task<List<GetUserByIdViewModel>> GetAllUsers()
        {
            var list = new List<GetUserByIdViewModel>();
            var users = await _unitOfWork.IRepositoryUser.GetByOrderAsync(x => x.Id != null, x => x.FullName, false);
            foreach (var user in users)
            {
                var data = await GetByIdAsync(user.Id);
                list.Add(data);
            }
            return list;
        }

        public async Task<List<GetUserByIdViewModel>> GetAllPartners()
        {
            var list = new List<GetUserByIdViewModel>();
            var users = await _unitOfWork.IRepositoryUser.GetByOrderAsync(x => x.IsPartner, x => x.FullName, false);
            foreach (var user in users)
            {
                var data = await GetByIdAsync(user.Id);
                list.Add(data);
            }
            return list;
        }

        public async Task<List<GetUserByIdViewModel>> GetAllUsersFilter(string value)
        {
            var list = await GetAllUsers();

            var listFiltered = list.Where(x => x.FullName.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                        || x.Email.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                        || x.Cpf.Contains(value, System.StringComparison.CurrentCultureIgnoreCase)
                                        || x.PhoneNumber.Contains(value, System.StringComparison.CurrentCultureIgnoreCase));

            return listFiltered.ToList();
        }

        public async Task<GetUserByIdViewModel> UpdateAdmin(UserAdminUpdateViewModel user, string idAdmin)
        {
            var userSelect = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(user.Id));
            userSelect.IsPartner = user.IsPartner;
            userSelect.IsAdmin = user.IsAdmin;
            await _unitOfWork.IRepositoryUser.UpdateAsync(userSelect);
            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(userSelect.Id);
        }

        public async Task<int> AmountAdmin()
        {
            var list = await _unitOfWork.IRepositoryUser.GetByAsync(x => x.IsActive && x.IsAdmin);
            return list.Count;
        }

        public async Task<int> AmountPartner()
        {
            var list = await _unitOfWork.IRepositoryUser.GetByAsync(x => x.IsActive && x.IsPartner);
            return list.Count;
        }

        public async Task<int> AmountUsers()
        {
            var list = await _unitOfWork.IRepositoryUser.GetByAsync(x => x.IsActive && !x.IsPartner && !x.IsAdmin);
            return list.Count;
        }

        public async Task<GetUsersAmountViewModel> TotalUsersAmount()
        {
            var admin = await AmountAdmin();
            var partner = await AmountPartner();
            var user = await AmountUsers();
            return new GetUsersAmountViewModel(admin, partner, user);
        }

        public async Task<UserBasicViewModel> GetByCpf(string Cpf)
        {
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Username.Equals(Cpf));
            if (user != null)
            {
                return new UserBasicViewModel(user.Id, user.FullName, user.IsAdmin, user.IsPartner, user.AcceptedTermsOfUse, user.IsActive);
            }
            else
            {
                return null;
            }
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

        public async Task AcceptTermsOfUse(string idUser)
        {
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(idUser));
            user.AcceptedTermsOfUse = true;
            await _unitOfWork.IRepositoryUser.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }

        private static readonly Regex digitsOnly = new Regex(@"[^\d]");
        private static readonly Regex noSpecialCharacters = new Regex(@"[\W_-[\s]]+");
        private static readonly Regex noNumbers = new Regex(@"\d+");
        public static string OnlyNumber(string document)
        {
            return digitsOnly.Replace(document, "");
        }

        public static string NoSpecialCharacters(string document)
        {
            var noCharSpecial = noSpecialCharacters.Replace(document, "");
            return noNumbers.Replace(noCharSpecial, "");
        }

        public static string RemoveAccents(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public string GetRoles(User user)
        {
            var listRoles = new List<string>();
            if (user.IsPartner)
                listRoles.Add("Parceiro");
            if (user.IsAdmin)
                listRoles.Add("Administrador");
            if(!user.IsPartner && !user.IsAdmin)
                listRoles.Add("Usuário");

            var result = String.Join(", ", listRoles);
            return result;
        }
    }
}
