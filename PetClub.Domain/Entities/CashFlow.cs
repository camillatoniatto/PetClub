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
        public CashFlow(string title, string description, string idUserCreate, string? idPurchaseOrder, string? idPaymentMethod, decimal launchValue, decimal netValue, DateTime expirationDate, DateTime writeOffDate, string idUserWriteOff, string idUserInactivate, bool isOutflow, DateTime writeDate)
        {
            Title = title;
            Description = description;
            IdUserCreate = idUserCreate;
            IdPurchaseOrder = idPurchaseOrder;
            IdPaymentMethod = idPaymentMethod;
            LaunchValue = launchValue;
            NetValue = netValue;
            ExpirationDate = expirationDate;
            WriteOffDate = writeOffDate;
            IdUserWriteOff = idUserWriteOff;
            IdUserInactivate = idUserInactivate;
            this.isOutflow = isOutflow;
            WriteDate = writeDate;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string IdUserCreate { get; set; }
        public User UserCreate { get; set; }

        public string? IdPurchaseOrder { get; set; }
        public string? IdPaymentMethod { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "O Preço é Obrigatório!")]
        public decimal LaunchValue { get; set; }
        public decimal NetValue { get; set; }

        public DateTime ExpirationDate { get; set; }
        public DateTime WriteOffDate { get; set; }

        public string IdUserWriteOff { get; set; }
        public string IdUserInactivate { get; set; }
        public bool isOutflow { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
