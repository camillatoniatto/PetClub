namespace PetClub.AppService.ViewModels.CashFlow
{
    public class ResumeCashflowViewModel
    {
        public ResumeCashflowViewModel(string entrada, string saida, string saldo)
        {
            Entrada = entrada;
            Saida = saida;
            Saldo = saldo;
        }

        public string Entrada { get; set; }
        public string Saida { get; set; }
        public string Saldo { get; set; }
    }
}
