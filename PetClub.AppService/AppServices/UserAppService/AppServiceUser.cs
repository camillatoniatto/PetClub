﻿using AutoMapper;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.Image.Model;
using PetClub.AppService.ViewModels.Account;
using PetClub.AppService.ViewModels.User;
using PetClub.Domain.Entities;
using PetClub.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task<UserUpdateViewModel> UpdateAsync(UpdatePerfilUserViewModel updatePerfilUserView, string IdUser)
        {
            var img = new EventImage();
            img.Value = updatePerfilUserView.Image;

            string urlImage = "PetClub";
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(IdUser));

            //if (!string.IsNullOrEmpty(user.Imagem))
            //{
            //    urlImage = await _appServiceAwsS3.UploadImageToS3(img, "User");
            //    await _appServiceAwsS3.DeleteAsync(user.Imagem);
            //}
            var date = DateTime.MinValue;

            user.FullName = updatePerfilUserView.Name != null ? updatePerfilUserView.Name : user.FullName;
            user.Image = urlImage != null ? urlImage : user.Image;
            user.Email = updatePerfilUserView.Email != null ? updatePerfilUserView.Email : user.Email;
            user.Birthdate = updatePerfilUserView.Birthdate != date ? updatePerfilUserView.Birthdate : user.Birthdate;
            user.PhoneNumber = updatePerfilUserView.PhoneNumber != null ? updatePerfilUserView.PhoneNumber : user.PhoneNumber;
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

        public async Task<GetUserByIdViewModel> GetByIdAsync(string Id)
        {
            CultureInfo culture = new CultureInfo("pt-BR");

            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Id.Equals(Id));
            var pets = await _appServicePet.GetPetsUser(Id);
            var roles = GetRoles(user);

            return new GetUserByIdViewModel(user.Id, user.FullName, user.Cpf, user.Email, user.PhoneNumber, user.Birthdate.ToString("d", culture),
                user.Image, user.IsAdmin, user.IsPartner, user.AddressName, user.Number, user.Complement, user.Neighborhood,
                user.City, user.State, user.ZipCode, pets, user.IsActive, roles, user.WriteDate);
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
            var user = await _unitOfWork.IRepositoryUser.GetByIdAsync(x => x.Cpf.Equals(Cpf));
            if (user != null)
            {
                return new UserBasicViewModel(user.Id, user.FullName, user.IsAdmin, user.IsPartner, user.AcceptedTermsOfUse, user.IsActive);
            }
            else
            {
                return null;
            }
        }


        //public async Task<string> ExportUserManagementEuphoria()
        //{
        //    // ajustando para exportar TODOS os usuarios
        //    var list = new List<UserListExportEuphoriaViewModel>();
        //    var users = await _unitOfWork.IRepositoryUser.GetByAsync(x => x.Id != null);
        //    foreach (var user in users)
        //    {
        //        var athletic = await _unitOfWork.IRepositoryAthletic.GetByIdAsync(X => X.Id.Equals(user.IdAthletic));
        //        list.Add(new UserListExportEuphoriaViewModel(user.FullName, user.UserName, user.Email, user.Birthdate.ToShortDateString(), user.PhoneNumber, user.IsActive ? "Ativo" : "Inativo", athletic != null ? athletic.CompanyName : string.Empty));
        //    }

        //    return await _appServiceExportFile.ExportExcelJson(list);
        //}

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
