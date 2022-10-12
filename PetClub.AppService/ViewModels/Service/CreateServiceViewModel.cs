using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Service
{
    public class CreateServiceViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ServiceType { get; set; }
        public bool SingleUse { get; set; }
        public DateTime DateDuration { get; set; }
        public decimal Value { get; set; }
    }
}
