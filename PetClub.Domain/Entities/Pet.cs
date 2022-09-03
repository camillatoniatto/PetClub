using PetClub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.Domain.Enum;

namespace PetClub.Domain.Entities
{
    public class Pet : EntityBase
    {
        public string IdUser { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public string Specie { get; set; }
        public string Brand { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsAlive { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
