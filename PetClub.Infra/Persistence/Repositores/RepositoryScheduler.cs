using PetClub.Domain.Entities;
using PetClub.Domain.Interfaces;
using PetClub.Infra.Persistence.Repositores.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Infra.Persistence.Repositores
{
    public class RepositoryScheduler : RepositoryBase<Scheduler>, IRepositoryScheduler
    {
        public RepositoryScheduler(PetClubContext context) : base(context)
        {
        }
    }
}
