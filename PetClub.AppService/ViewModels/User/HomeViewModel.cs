using PetClub.AppService.ViewModels.Pet;

namespace PetClub.AppService.ViewModels.User
{
    public class HomeViewModel
    {
        public HomeViewModel(int agendamentos, int pet, int aniversario, List<AniversarioPetViewModel> petsAniversario = null)
        {
            Agendamentos = agendamentos;
            Pet = pet;
            Aniversario = aniversario;
            PetsAniversario = petsAniversario;
        }

        public int Agendamentos { get; set; }
        public int Pet { get; set; }
        public int Aniversario { get; set; }
        public List<AniversarioPetViewModel> PetsAniversario { get; set; }
    }

    public class AniversarioPetViewModel
    {
        public AniversarioPetViewModel(string idPet, string userFullName, string name, string genreString, string specie, string brand, string birthdate)
        {
            IdPet = idPet;
            UserFullName = userFullName;
            Name = name;
            GenreString = genreString;
            Specie = specie;
            Brand = brand;
            Birthdate = birthdate;
        }

        public string IdPet { get; set; }
        public string UserFullName { get; set; }
        public string Name { get; set; }
        public string GenreString { get; set; }
        public string Specie { get; set; }
        public string Brand { get; set; }
        public string Birthdate { get; set; }        
    }

}
