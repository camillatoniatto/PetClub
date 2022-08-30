using PetClub.Domain.Interfaces;
using PetClub.Infra.Persistence.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Infra.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PetClubContext _context;
        private RepositoryUser _repositoryUser;
        private RepositoryRefreshTokenData _repositoryRefreshTokenData;

        public UnitOfWork(PetClubContext context)
        {
            _context = context;
        }

        public IRepositoryUser IRepositoryUser => _repositoryUser ??= new RepositoryUser(_context);
        public IRepositoryRefreshTokenData IRepositoryRefreshTokenData => _repositoryRefreshTokenData ??= new RepositoryRefreshTokenData(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
