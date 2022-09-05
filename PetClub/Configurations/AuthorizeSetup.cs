using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClub.Configurations
{
    public static class AuthorizeSetup
    {
        // CLAIM TYPE
        public const string CLAIM_TYPE_OCCUPATION = "Occupation";
        
        // GRUPOS DE AUTORIZAÇÃO        
        public const string ADMIN_SYSTEM = "admin_system, partner, user";
        public const string PARTER = "partner, user";
        public const string USER = "user";
    }
}
