using AutoMapper;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.ImageAppService
{
    public class AppServiceImage : IAppServiceImage
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifierAppService _notifier;

        public AppServiceImage(IUnitOfWork unitOfWork, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _notifier = notifier;
        }
    }
}
