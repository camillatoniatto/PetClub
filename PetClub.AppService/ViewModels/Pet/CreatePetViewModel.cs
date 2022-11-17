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
        public string idUser { get; set; }
        public string Name { get; set; }
        public int Genre { get; set; }
        public string Specie { get; set; }
        public string Brand { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
