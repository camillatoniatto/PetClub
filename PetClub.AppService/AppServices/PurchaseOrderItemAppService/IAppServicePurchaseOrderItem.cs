using PetClub.AppService.ViewModels.PurchaseOrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.PurchaseOrderItemAppService
{
    public interface IAppServicePurchaseOrderItem
    {
        Task<string> AddPurchaseOrderItem(CreatePurchaseOrderItemViewModel model);
        Task<List<GetOrderItensViewModel>> GetOrderItens(string idPurchaseOrder);
        Task UpdadeOrDeleteItem(UpdatePurchaseOrderItemViewModel model, bool upd);
    }
}
