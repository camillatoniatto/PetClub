using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.User
{
    public class RefreshTokenDataViewModel
    {
        public RefreshTokenDataViewModel(string idUser, string refreshToken)
        {
            IdUser = idUser;
            RefreshToken = refreshToken;
        }

        public string IdUser { get; set; }
        public string RefreshToken { get; set; }
    }
}
