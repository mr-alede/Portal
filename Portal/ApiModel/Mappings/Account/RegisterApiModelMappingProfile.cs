using AutoMapper;
using Portal.ApiModel.Account;
using Portal.Model.Identity;

namespace Portal.ApiModel.Mappings.Account
{
    public class RegisterApiModelMappingProfile : Profile
    {
        public RegisterApiModelMappingProfile()
        {
            CreateMap<RegisterApiModel, PortalUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
