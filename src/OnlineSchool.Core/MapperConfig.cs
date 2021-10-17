
using AutoMapper;
using OnlineSchool.Core.MappingProfiles;

namespace OnlineSchool.Core
{
    public static class MapperConfig
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DocumentMappingProfile());
            });

            return config;
        }
    }
}
