using PetClub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class RefreshTokenData : EntityBase
    {

        public RefreshTokenData(string idUser, string refreshToken)
        {
            IdUser = idUser;
            RefreshToken = refreshToken;
        }

        public string IdUser { get; set; }
        public User User { get; set; }
        public string RefreshToken { get; set; }
    }
}
