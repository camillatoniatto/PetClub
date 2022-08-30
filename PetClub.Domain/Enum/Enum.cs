using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PetClub.Domain.Enum
{
    public class Enum
    {
        public enum RecordSituation
        {
            [Display(Name = "Ativo")]
            ACTIVE,
            [Display(Name = "Inativo")]
            INACTIVE
        }
    }
}
