using FluentValidation;
using InsurancePolicies.Core.Entities;
using InsurancePolicies.Infrastructure;
using System;
using System.Linq;

namespace InsurancePolicies.API.Validators
{
    public class InsurancePolicyValidator : AbstractValidator<InsurancePolicy>
    {
        public InsurancePolicyValidator()
        {
            RuleFor(x => x.FirstName).MinimumLength(2).WithMessage("Invalid first name");
            RuleFor(x => x.LastName).MinimumLength(2).WithMessage("Invalid last name");
            
            RuleFor(x => x.Address).Custom((address, context) => 
            {
                var usAddressess = new USAddresses().ValidUSAddresses;
                 if (address == null || !usAddressess.Where(a => (a.StateId == address.State || a.StateName == address.State) && a.City == address.City && a.ZipCode == address.ZipCode).Any())
                {
                    context.AddFailure("Not valid US-based format");
                }
            });

            RuleFor(x => x.EffectiveDate).Custom((effectiveDate, context) =>
           {
               if ((effectiveDate - DateTime.Now).Days < 30)
               {
                   context.AddFailure("The effective date must be at least 30 days in the future");
               }
           });

            RuleFor(x => x.VehicleDetail.Year).LessThan(1998).WithMessage("Classic vehicle not met");
        }
    }
}
