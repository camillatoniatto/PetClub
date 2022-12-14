using PetClub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.ViewModels.Pet
{
    public class GetPetViewModel
    {
        public GetPetViewModel(string idPet, string idUser, string tutor, string name, int genre, string genreString, string specie, string brand, string birthdate, bool isAlive, string writeDate, DateTime birthdateDate)
        {
            IdPet = idPet;
            IdUser = idUser;
            Tutor = tutor;
            Name = name;
            Genre = genre;
            GenreString = genreString;
            Specie = specie;
            Brand = brand;
            Birthdate = birthdate;
            IsAlive = isAlive;
            WriteDate = writeDate;
            BirthdateDate = birthdateDate;
        }

        public string IdPet { get; set; }
        public string IdUser { get; set; }
        public string Tutor { get; set; }
        public string Name { get; set; }
        public int Genre { get; set; }
        public string GenreString { get; set; }
        public string Specie { get; set; }
        public string Brand { get; set; }
        public string Birthdate { get; set; }
        public DateTime BirthdateDate { get; set; }
        public bool IsAlive { get; set; }
        public string WriteDate { get; set; }
    }
}
