using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.UsersPartners
{
    public class GetUsersPartnersViewModel
    {
        public GetUsersPartnersViewModel(string id, string idUser, string userFullName, string userCpf, string userEmail, string userPhone, string idPartner, string partnerFullName, string parnerCpf, string dateCreation, int quantityPet, string birthdate, string addressName, string number, string complement, string neighborhood, string city, string state, string zipCode)
        {
            Id = id;
            IdUser = idUser;
            UserFullName = userFullName;
            UserCpf = userCpf;
            UserEmail = userEmail;
            UserPhone = userPhone;
            IdPartner = idPartner;
            PartnerFullName = partnerFullName;
            ParnerCpf = parnerCpf;
            DateCreation = dateCreation;
            QuantityPet = quantityPet;
            Birthdate = birthdate;
            AddressName = addressName;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Id { get; set; }
        public string IdUser { get; set; }
        public string UserFullName { get; set; }
        public string UserCpf { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string IdPartner { get; set; }
        public string PartnerFullName { get; set; }
        public string ParnerCpf { get; set; }
        public string DateCreation { get; set; }
        public int QuantityPet { get; set; }
        public string Birthdate { get; set; }
        public string AddressName { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }
}
