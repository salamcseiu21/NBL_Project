using AutoMapper;

namespace NblClassLibrary.Models.AutoMapper
{
    public  class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainProfile>();
            });
        }
    }
}
