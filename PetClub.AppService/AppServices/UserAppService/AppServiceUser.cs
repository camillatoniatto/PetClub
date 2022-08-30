using AutoMapper;
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.UserAppService
{
    public class AppServiceUser : IAppServiceUser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotifierAppService _notifier;

        public AppServiceUser(IUnitOfWork unitOfWork, IMapper mapper, INotifierAppService notifier)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notifier = notifier;
        }
    }
}
