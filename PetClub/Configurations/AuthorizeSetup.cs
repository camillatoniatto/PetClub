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
        public const string ADMIN_SYSTEM = "admin_system";
        public const string PARTNER = "admin_system, partner";
        public const string USER = "admin_system, partner, user";

        // PERFIS
        public const string PROFILE_ADMIN = "admin_system";
        public const string PROFILE_PARTNER = "partner";
        public const string PROFILE_USER = "user";
    }
}
