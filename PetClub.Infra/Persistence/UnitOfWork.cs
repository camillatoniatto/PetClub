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
        private RepositoryCashFlow _repositoryCashFlow;
        private RepositoryPaymentMethod _repositoryPaymentMethod;
        private RepositoryPet _repositoryPet;
        private RepositoryPurchaseOrder _repositoryPurchaseOrder;
        private RepositoryPurchaseOrderItem _repositoryPurchaseOrderItem;
        private RepositoryScheduler _repositoryScheduler;
        private RepositoryService _repositoryService;
        private RepositoryUsersPartners _repositoryUsersPartners;

        public UnitOfWork(PetClubContext context)
        {
            _context = context;
        }
        public IRepositoryCashFlow IRepositoryCashFlow => _repositoryCashFlow ??= new RepositoryCashFlow(_context);
        public IRepositoryPaymentMethod IRepositoryPaymentMethod => _repositoryPaymentMethod ??= new RepositoryPaymentMethod(_context);
        public IRepositoryPet IRepositoryPet => _repositoryPet ??= new RepositoryPet(_context);
        public IRepositoryPurchaseOrder IRepositoryPurchaseOrder => _repositoryPurchaseOrder ??= new RepositoryPurchaseOrder(_context);
        public IRepositoryPurchaseOrderItem IRepositoryPurchaseOrderItem => _repositoryPurchaseOrderItem ??= new RepositoryPurchaseOrderItem(_context);
        public IRepositoryRefreshTokenData IRepositoryRefreshTokenData => _repositoryRefreshTokenData ??= new RepositoryRefreshTokenData(_context);
        public IRepositoryScheduler IRepositoryScheduler => _repositoryScheduler ??= new RepositoryScheduler(_context);
        public IRepositoryService IRepositoryService => _repositoryService ??= new RepositoryService(_context);
        public IRepositoryUser IRepositoryUser => _repositoryUser ??= new RepositoryUser(_context);
        public IRepositoryUsersPartners IRepositoryUsersPartners => _repositoryUsersPartners ??= new RepositoryUsersPartners(_context);

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
