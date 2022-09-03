using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PetClub.Domain.Enum
{
    public enum RecordSituation
    {
        [Display(Name = "Ativo")]
        ACTIVE,
        [Display(Name = "Inativo")]
        INACTIVE
    }

    public enum Genre
    {
        [Display(Name = "Macho")]
        MALE,
        [Display(Name = "Fêmea")]
        FEMALE
    }

    public enum ServiceType
    {
        [Display(Name = "Hospedagem")]
        HOST,
        [Display(Name = "Passeio")]
        WALK_DOG,
        [Display(Name = "Serviço Veterinário")]
        VET_SERVICE,
        [Display(Name = "Banho e Tosa")]
        PET_GROOMING,
        [Display(Name = "Outros")]
        OTHER
    }

    public enum PaymentType
    {
        [Display(Name = "Cartão de crédito")]
        CREDIT_CARD,
        [Display(Name = "Dinheiro")]
        CASH,
        [Display(Name = "Cartão de Débito")]
        DEBIT,
        [Display(Name = "Pix")]
        PIX,
    }

    public enum PurchaseOrderSituation
    {
        [Display(Name = "Pendente")]
        PENDING,
        [Display(Name = "Concluido")]
        CONCLUDEDD,
        [Display(Name = "Cancelado")]
        CANCELED
    }

    public enum PaymentSituation
    {
        [Display(Name = "Pendente")]
        PENDING,
        [Display(Name = "Aprovado")]
        APPROVED,
        [Display(Name = "Cancelado")]
        CANCELED,
        [Display(Name = "Estornado")]
        VOID
    }

    public enum SchedulerSituation
    {
        [Display(Name = "Agendado")]
        SCHEDULED,
        [Display(Name = "Concluido")]
        CONCLUDEDD,
        [Display(Name = "Cancelado")]
        CANCELED, 
        [Display(Name = "Em Atendimento")]
        IN_SERVICE,

    }
}
