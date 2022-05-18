using FluentValidation;
using InsurancePolicies.Core.Entities;
using System;

namespace InsurancePolicies.API.Validators
{
    public class InsurancePolicyValidator : AbstractValidator<InsurancePolicy>
    {
        public InsurancePolicyValidator()
        {
            RuleFor(x => x.FirstName).MinimumLength(2).WithMessage("Invalid first name");
            RuleFor(x => x.LastName).MinimumLength(2).WithMessage("Invalid last name");
            RuleFor(x => x.Address.Street).MinimumLength(3).WithMessage("Invalid street");
            RuleFor(x => x.Address.City).MinimumLength(3).WithMessage("Invalid city");
            RuleFor(x => x.Address.State).MinimumLength(2).WithMessage("Invalid state");
            RuleFor(x => x.Address.Country).MinimumLength(3).WithMessage("Invalid country");
            RuleFor(x => x.Address.ZipCode).MinimumLength(5).WithMessage("Invalid zip code");

            RuleFor(x => x.EffectiveDate).Custom((effectiveDate, context) =>
           {
               if ((effectiveDate - DateTime.Now).Days < 30)
               {
                   context.AddFailure("The effective date must be at least 30 days in the future");
               }
           });

            RuleFor(x => x.VehicleDetail.Year).LessThan(1998).WithMessage("Classic vehicle not meet");
        }
    }
}
