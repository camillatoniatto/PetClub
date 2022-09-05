using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Account
{
    public  class UserBasicViewModel
    {
        public UserBasicViewModel(string id, string fullName, bool isAdmin, bool isPartner, bool acceptedTermsOfUse, bool isActive)
        {
            Id = id;
            FullName = fullName;
            IsAdmin = isAdmin;
            IsPartner = isPartner;
            AcceptedTermsOfUse = acceptedTermsOfUse;
            IsActive = isActive;
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }
        public bool AcceptedTermsOfUse { get; set; }
        public bool IsActive { get; set; }
    }
}
