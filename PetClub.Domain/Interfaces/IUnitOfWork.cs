using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
        IRepositoryUser IRepositoryUser { get; }
        IRepositoryRefreshTokenData IRepositoryRefreshTokenData { get; }

    }
}
