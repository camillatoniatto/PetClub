using Microsoft.EntityFrameworkCore.ChangeTracking;
using PetClub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Entities
{
    public class CashFlow : EntityBase
    {
        public string Title { get; set; }

        public string IdUserCreate { get; set; }
        public User UserCreate { get; set; }

        public string? IdPurchaseOrder { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public string? IdPaymentMethod { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "O Preço é Obrigatório!")]
        public decimal LaunchValue { get; set; }
        public decimal NetValue { get; set; }

        public DateTime ExpirationDate { get; set; }
        public DateTime? WriteOffDate { get; set; }

        public string IdUserWriteOff { get; set; }
        public User UserWriteOff { get; set; }

        public string IdUserInactivate { get; set; }
        public User UserInactivate { get; set; }

        public DateTime WriteDate { get; set; }
    }
}
