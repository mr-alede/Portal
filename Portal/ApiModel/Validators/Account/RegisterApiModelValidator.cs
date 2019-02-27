using FluentValidation;
using Portal.ApiModel.Account;

namespace Portal.ApiModel.Validators.Account
{
    public class RegisterApiModelValidator : AbstractValidator<RegisterApiModel>
    {
        public RegisterApiModelValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.FirstName).NotEmpty().WithMessage("FirstName cannot be empty");
            RuleFor(vm => vm.LastName).NotEmpty().WithMessage("LastName cannot be empty");
        }
    }
}
