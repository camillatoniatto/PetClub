using Microsoft.IdentityModel.Tokens;
using PetClub.CrossCutting.Identity.Interfaces;
using PetClub.CrossCutting.Identity.Models;
using PetClub.CrossCutting.Identity.ViewModel;
using PetClub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.CrossCutting.Identity.Services
{
    public class TokenService : ITokenService
    {
        public LoginResponseViewModel GenerateToken(ClaimsIdentity claimsIdentity, ApplicationUser user, string refreshToken, bool isAdmin = false, bool isPartner = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSerttingsSecurity.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedtoken = tokenHandler.WriteToken(token);
            var response = new LoginResponseViewModel
            {
                AccessToken = encodedtoken,
                RefreshToken = refreshToken,
                ExpiresIn = TimeSpan.FromDays(7).TotalSeconds,
                User = new UserTokenViewModel
                {
                    IdUser = user.Id,
                    FullName = user.FullName,
                    Cpf = user.Cpf,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Birthdate = user.Birthdate,
                    Image = user.Image,
                    IsAdmin = isAdmin,
                    IsPartner = isPartner
                }
            };
            return response;
        }
    }
}
