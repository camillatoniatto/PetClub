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
        IRepositoryCashFlow IRepositoryCashFlow { get; }
        IRepositoryPaymentMethod IRepositoryPaymentMethod { get; }
        IRepositoryPet IRepositoryPet { get; }
        IRepositoryPurchaseOrder IRepositoryPurchaseOrder { get; }
        IRepositoryPurchaseOrderItem IRepositoryPurchaseOrderItem { get; }
        IRepositoryScheduler IRepositoryScheduler { get; }
        IRepositoryService IRepositoryService { get; }
        IRepositoryUsersPartners IRepositoryUsersPartners { get; }
    }
}
