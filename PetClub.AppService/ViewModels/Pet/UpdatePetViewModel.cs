using PetClub.Domain.Enum;

namespace PetClub.AppService.ViewModels.Pet
{
    public class UpdatePetViewModel
    {
        public string IdPet { get; set; }
        public string Name { get; set; }
        public int Genre { get; set; }
        public string Specie { get; set; }
        public string Brand { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsAlive { get; set; }
    }
}
