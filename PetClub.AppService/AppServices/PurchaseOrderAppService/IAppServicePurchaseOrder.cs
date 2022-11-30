using PetClub.AppService.ViewModels.PurchaseOrder;
using PetClub.AppService.ViewModels.PurchaseOrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.PurchaseOrderAppService
{
    public interface IAppServicePurchaseOrder
    {
        Task<string> CreatePurchaseOrder(CreatePurchaseOrderViewModel model, string idPARTNER);
        Task<GetPurchaseOrderViewModel> GetPurchaseOrderById(string idPurchaseOrder);
        Task<List<GetPurchaseOrderViewModel>> GetPurchaseOrdersUser(string idUser, bool isApp);
        Task UpdadePurchaseOrder(UpdatePurchaseOrderViewModel model);
        Task DeletePurchaseOrder(string idPurchaseOrder);
        Task ConcluedPurchaseOrder(string idPurchaseOrder, bool isPaid);
        Task<string> GetValue(List<CreatePurchaseOrderItemViewModel> itens);
    }
}
