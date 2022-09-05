using PetClub.Domain.Enum;

namespace PetClub.AppService.ViewModels.Service
{
    public class UpdateServiceViewModel
    {
        public string IdService { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ServiceType ServiceType { get; set; }
        public bool SingleUse { get; set; }
        public DateTime DateDuration { get; set; }
        public decimal Value { get; set; }
    }
}
