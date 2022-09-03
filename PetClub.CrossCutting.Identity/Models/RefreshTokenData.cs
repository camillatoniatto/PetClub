using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.Domain.Enum;

namespace PetClub.CrossCutting.Identity.Models
{
    public class RefreshTokenData
    {
        protected RefreshTokenData()
        {
            Id = Guid.NewGuid().ToString();
            DateCreation = DateTime.Now.ToBrasilia();
            RecordSituation = RecordSituation.ACTIVE;
        }

        public RefreshTokenData(string idApplicationUser, string refreshToken)
        {
            IdApplicationUser = idApplicationUser;
            RefreshToken = refreshToken;
        }

        public string Id { get; set; }
        public DateTime DateCreation { get; set; }
        public RecordSituation RecordSituation { get; set; }
        public string IdApplicationUser { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string RefreshToken { get; set; }
    }
}
