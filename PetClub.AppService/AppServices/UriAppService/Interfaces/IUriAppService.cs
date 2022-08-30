using PetClub.AppService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.UriAppService.Interfaces
{
    public interface IUriAppService
    {
        Uri GetAllUri(PagingParametersViewModel pagination = null);
    }
}
