using PetClub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class UsersPartners : EntityBase
    {
        public UsersPartners(string idPartner,string idUser, DateTime writeDate)
        {
            IdPartner = idPartner;
            IdUser = idUser;
            WriteDate = writeDate;
        }

        public string IdPartner { get; set; }
        public User User { get; set; }
        public string IdUser { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
