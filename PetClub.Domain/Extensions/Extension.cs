using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Domain.Extensions
{
    public static class Extension
    {
        public static DateTime ToBrasilia(this DateTime data)
        {
            DateTime timeUtc = DateTime.UtcNow;
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            TimeZoneInfo kstZone;
            if (isWindows)
            {
                kstZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
            else
            {
                kstZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }

            DateTime dateTimeBrasilia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, kstZone);
            return dateTimeBrasilia;
        }
    }
}
