using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PetClub.AppService.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClub.Configurations
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingConfiguration());
                mc.AllowNullCollections = true;
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
