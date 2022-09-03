using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.UsersPartners
{
    public class GetUsersPartnersViewModel
    {
        public GetUsersPartnersViewModel(string id, string idUser, string userFullName, string userCpf, string userEmail, string userPhone, string idPartner, string partnerFullName, string parnerCpf)
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

    }
}
