using AutoMapper;
using CoreServices.Handler.DTO;
using CoreServices.Infrastructure.Repositories.DbModels;

namespace CoreServices.Handler.MappingPorfile
{
    public class CoreServiceProfile:Profile
    {
        public CoreServiceProfile()
        {
            CreateMap<PostDb, PostDto>(MemberList.Source);
            CreateMap<CategoryDb, CategoryDto>(MemberList.Source);
        }
    }
}
