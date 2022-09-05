using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Pet
{
    public class CreatePetViewModel
    {
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public string Specie { get; set; }
        public string Brand { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
