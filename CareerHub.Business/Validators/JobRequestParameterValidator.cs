using CareerHub.Core.Parameters;
using FluentValidation;

namespace CareerHub.Business.Validators
{
    public class JobRequestParameterValidator : AbstractValidator<JobRequestModel>
    {
        public JobRequestParameterValidator()
        {           
            RuleFor(p => p.Position).NotEmpty().NotNull().WithMessage("Pozisyon alanı zorunludur!");
            RuleFor(p => p.Description).NotEmpty().NotNull().WithMessage("İlan açılaması alanı zorunludur!");
        }
    }
}
