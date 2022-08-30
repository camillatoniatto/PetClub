using PetClub.Domain.Interfaces;
using PetClub.Domain.Entities;
using PetClub.Infra.Persistence.Repositores.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Infra.Persistence.Repositores
{
    public class RepositoryRefreshTokenData : RepositoryBase<RefreshTokenData>, IRepositoryRefreshTokenData
    {
        public RepositoryRefreshTokenData(PetClubContext context) : base(context)
        {
        }
    }
}
