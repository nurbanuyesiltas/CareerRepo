using CareerHub.Core.Parameters;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CareerHub.Business.Validators
{
    public class CompanyRequestParameterValidator : AbstractValidator<CompanyRequestModel>
    {
        public CompanyRequestParameterValidator()
        {
            RuleFor(p => p.PhoneNumber).NotEmpty().NotNull().WithMessage("Telefon numarası alanı zorunludur!").Matches(new Regex(@"^(05(\d{9}))$"))
                .WithMessage("Telefon numarası alanında sadece sayı girebilirsiniz");
            RuleFor(p => p.CompanyName).NotEmpty().NotNull().WithMessage("Firma adı alanı zorunludur!");
            RuleFor(p => p.Address).NotEmpty().NotNull().WithMessage("Adres alanı zorunludur!");

        }
    }
}
