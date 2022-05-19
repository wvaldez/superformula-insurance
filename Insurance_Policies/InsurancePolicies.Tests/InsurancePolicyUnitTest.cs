using FluentValidation.TestHelper;
using InsurancePolicies.API.Validators;
using InsurancePolicies.Core.Entities;
using InsurancePolicies.Core.ValueObjects;
using InsurancePolicies.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InsurancePolicies.Tests
{
    public class InsurancePolicyUnitTest
    {
        private readonly InsurancePolicy _validPolicy;
        public InsurancePolicyUnitTest()
        {
            _validPolicy = new InsurancePolicy();
            _validPolicy.FirstName = "Walterio";
            _validPolicy.LastName = "Valdez";
            _validPolicy.EffectiveDate = DateTime.Now.AddMonths(2);
            _validPolicy.Address = new Address { City = "Lowell", Country = "USA", State = "Massachusetts", Street = "", ZipCode = "01850" };
            _validPolicy.VehicleDetail = new Vehicle { Model = "Impala", Year = 1967, Manufacturer = "Chevrolet", Name = "Supernatural" };
        }

        [Fact]
        public void Should_be_a_valid_effective_date()
        {
            // Act
            var result = new InsurancePolicyValidator().Validate(_validPolicy);

            // Assert
            Assert.True(result.IsValid);
            Assert.DoesNotContain("The effective date must be at least 30 days in the future", result.Errors.Select(x => x.ErrorMessage));
        }


        [Fact]
        public void Should_be_an_invalid_effective_date()
        {
            // Arrange
            InsurancePolicy policy = new InsurancePolicy();
            policy.FirstName = "Walterio";
            policy.LastName = "Valdez";
            policy.EffectiveDate = DateTime.Now;
            policy.VehicleDetail = new Vehicle { Model = "Impala", Year = 1967, Manufacturer = "Chevrolet", Name = "Supernatural" };
            policy.Address = new Address { City = "Merrimac", Country = "USA", State = "Massachusetts", Street = "", ZipCode = "01860" };

            // Act
            var result = new InsurancePolicyValidator().Validate(policy);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The effective date must be at least 30 days in the future", result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public void Should_be_a_valid_vehicle_year()
        {
            // Act
            var result = new InsurancePolicyValidator().Validate(_validPolicy);

            // Assert
            Assert.True(result.IsValid);
            Assert.DoesNotContain("Classic vehicle not met", result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public void Should_be_an_invalid_vehicle_year()
        {
            // Arrange
            InsurancePolicy policy = new InsurancePolicy();
            policy.FirstName = "Walterio";
            policy.LastName = "Valdez";
            policy.EffectiveDate = DateTime.Now;
            policy.VehicleDetail = new Vehicle { Model = "Impala", Year = 2022, Manufacturer = "Chevrolet", Name = "Supernatural" };
            policy.Address = new Address { City = "Merrimac", Country = "USA", State = "Massachusetts", Street = "", ZipCode = "01860" };

            // Act
            var result = new InsurancePolicyValidator().Validate(policy);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Classic vehicle not met", result.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public void Should_be_a_valid_US_Address()
        {
            // Act
            var result = new InsurancePolicyValidator().Validate(_validPolicy);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_be_an_invalid_US_Address()
        {
            // Arrange
            InsurancePolicy policy = new InsurancePolicy();
            policy.FirstName = "Walterio";
            policy.LastName = "Valdez";
            policy.EffectiveDate = DateTime.Now.AddMonths(2);
            policy.Address = new Address { City = "Cancun", Country = "MEX", State = "Quintana Roo", Street = "Circuito Almeria", ZipCode = "77510" };
            policy.VehicleDetail = new Vehicle { Model = "Impala", Year = 1967, Manufacturer = "Chevrolet", Name = "Supernatural" };

            var address = policy.Address;

            // Act
            var result = new InsurancePolicyValidator().Validate(policy);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Not valid US-based format", result.Errors.Select(x => x.ErrorMessage));
        }
    }
}
