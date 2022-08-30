using PetClub.CrossCutting.Identity.Models;
using PetClub.CrossCutting.Identity.ViewModel;
using PetClub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.CrossCutting.Identity.Interfaces
{
    public interface ITokenService
    {
        LoginResponseViewModel GenerateToken(ClaimsIdentity claimsIdentity, ApplicationUser user, string refreshToken, bool isAdmin = false, bool isPartner = false);
    }
}
