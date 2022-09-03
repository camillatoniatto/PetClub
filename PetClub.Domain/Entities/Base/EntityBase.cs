using PetClub.Domain.Enum;
using PetClub.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.Domain.Enum;

namespace PetClub.Domain.Entities.Base
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid().ToString();
            DateCreation = DateTime.Now.ToBrasilia();
            RecordSituation = RecordSituation.ACTIVE;
        }

        public string Id { get; set; }

        public DateTime DateCreation { get; set; }

        public RecordSituation RecordSituation { get; set; }
    }
}
