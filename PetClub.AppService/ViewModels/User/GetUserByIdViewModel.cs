using PetClub.AppService.ViewModels.Pet;
using PetClub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.User
{
    public class GetUserByIdViewModel
    {
        public GetUserByIdViewModel(string id, string fullName, string cpf, string email, string phoneNumber, DateTime birthdate, string image, bool isAdmin, bool isPartner, string addressName, string number, string complement, string neighborhood, string city, string state, string zipCode, IList<GetPetViewModel> pet, bool isActive, DateTime writeDate)
        {
            Id = id;
            FullName = fullName;
            Cpf = cpf;
            Email = email;
            PhoneNumber = phoneNumber;
            Birthdate = birthdate;
            Image = image;
            IsAdmin = isAdmin;
            IsPartner = isPartner;
            AddressName = addressName;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
            Pet = pet;
            IsActive = isActive;
            WriteDate = writeDate;
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public string Image { get; set; }

        // roles
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }

        // endereço
        public string AddressName { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public IList<GetPetViewModel> Pet { get; set; }
        public bool IsActive { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
