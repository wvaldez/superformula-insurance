using FluentValidation;
using InsurancePolicies.Core.Entities;
using System;

namespace InsurancePolicies.API.Validators
{
    public class InsurancePolicyValidator: AbstractValidator<InsurancePolicy>
    {
        public InsurancePolicyValidator()
        {
            RuleFor(x => x.EffectiveDate).Custom((effectiveDate, context) =>
           {
               if ((effectiveDate - DateTime.Now).Days < 30)
               {
                   context.AddFailure("The effective date must be at least 30 days in the future");
               }
           });
        }
    }
}
