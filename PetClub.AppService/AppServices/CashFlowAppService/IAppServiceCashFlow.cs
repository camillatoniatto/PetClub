using PetClub.AppService.ViewModels.CashFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.CashFlowAppService
{
    public interface IAppServiceCashFlow
    {
        Task CreateReceivableBill(CreateCashFlowViewModel model, string idUser);
        Task DeleteBill(string idCashFlow, string idUser);
        Task<List<GetCashFlowViewModel>> GetCashFlow(string idPARTNER);
        Task<UpdateCashFlowViewModel> UpdateAsync(UpdateCashFlowViewModel model, string idUser);
        Task WriteOff(string idCashFlow, string idUser);
    }
}
