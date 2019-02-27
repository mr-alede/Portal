using AutoMapper;
using Portal.Model.Identity;
using Portal.ViewModels.ExternalAuth;

namespace Portal.ViewModels.Mappings.ExternalAuth
{
    public class ExternalLoginConfirmationViewModelMapping : Profile
    {
        public ExternalLoginConfirmationViewModelMapping()
        {
            CreateMap<ExternalLoginConfirmationViewModel, PortalUser>();
        }
    }

}
