namespace PetClub.AppService.ViewModels.User
{
    public class HomeViewModel
    {
        public HomeViewModel(int agendamentos, int pet, int aniversario)
        {
            Agendamentos = agendamentos;
            Pet = pet;
            Aniversario = aniversario;
        }

        public int Agendamentos { get; set; }
        public int Pet { get; set; }
        public int Aniversario { get; set; }
    }
}
