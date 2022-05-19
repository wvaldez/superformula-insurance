using InsurancePolicies.API.Controllers;
using InsurancePolicies.API.Models;
using InsurancePolicies.API.Services;
using InsurancePolicies.Core.Entities;
using InsurancePolicies.Core.IRepositories;
using InsurancePolicies.Core.ValueObjects;
using InsurancePolicies.Infrastructure.Database;
using InsurancePolicies.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InsurancePolicies.Tests
{
    public class InsurancePoliciesControllerTest
    {
        private readonly DbContextOptions<InsuranceDbContext> _dbContextOptions;
        private readonly InsurancePoliciesController _controller;
        private readonly List<InsurancePolicy> _fakeInsurancePoliciesContext;
        private readonly InsurancePolicy _validPolicy;

        public InsurancePoliciesControllerTest()
        {

            _fakeInsurancePoliciesContext = GetFakeInsurancePolicies();
            var logger = new Mock<ILogger<InsurancePoliciesController>>();
            var insurancePolicyRepository = new Mock<IInsurancePolicyRepository>();
            insurancePolicyRepository.Setup(x => x.GetAll()).Returns(_fakeInsurancePoliciesContext.AsQueryable());
            insurancePolicyRepository
                .Setup(x => x.AddAsync(It.IsAny<InsurancePolicy>()))
                .Callback((InsurancePolicy model) => { _fakeInsurancePoliciesContext.Add(model); })
                .Returns((InsurancePolicy model) => Task.FromResult((EntityEntry<InsurancePolicy>)null));
            var stateRegulator = new Mock<IStateRegulator>();
            stateRegulator
                .Setup(x => x.ValidRegulation(It.IsAny<InsurancePolicy>()))
                .Callback(() => Task.Delay(2000).Wait())
                .Returns(Task.FromResult(new StateRegulationResult { Reason = "Success regulation due to valid data", Status = true }));
            var messagePublisher = new Mock<IMessagePublisher>();


            _controller = new InsurancePoliciesController(logger.Object, insurancePolicyRepository.Object, stateRegulator.Object, messagePublisher.Object);

        }

        [Fact]
        public async Task Get_policies_by_license_number_success()
        {
            // Arrange
            var licenseNumber = "D00027";

            // Act
            var result = await _controller.GetPoliciesByLicenseNumber(licenseNumber);
            var insurancePoliciesResult = (OkObjectResult)result.Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((EnumerableQuery<InsurancePolicy>)insurancePoliciesResult.Value).Count());
        }

        [Fact]
        public async Task Get_policies_by_license_number_sort_by_vehicle_year()
        {
            // Arrange
            var licenseNumber = "D00027";

            // Act
            var result = await _controller.GetPoliciesByLicenseNumber(licenseNumber, true);
            var insurancePoliciesResult = (OkObjectResult)result.Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((EnumerableQuery<InsurancePolicy>)insurancePoliciesResult.Value).Count());
            Assert.Equal(3, ((EnumerableQuery<InsurancePolicy>)insurancePoliciesResult.Value).First().Id);
        }

        [Fact]
        public async Task Get_policies_by_license_number_including_expired()
        {
            // Arrange
            var licenseNumber = "D00054";

            // Act
            var result = await _controller.GetPoliciesByLicenseNumber(licenseNumber,includeExpiredPolicies:true);
            var insurancePoliciesResult = (OkObjectResult)result.Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, ((EnumerableQuery<InsurancePolicy>)insurancePoliciesResult.Value).Count());
            Assert.Equal(2, ((EnumerableQuery<InsurancePolicy>)insurancePoliciesResult.Value).First().Id);
        }

        [Fact]
        public async Task Get_policies_by_license_number_not_including_expired()
        {
            // Arrange
            var licenseNumber = "D00054";

            // Act
            var result = await _controller.GetPoliciesByLicenseNumber(licenseNumber);
            var insurancePoliciesResult = (OkObjectResult)result.Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, ((EnumerableQuery<InsurancePolicy>)insurancePoliciesResult.Value).Count());
        }

        [Fact]
        public async Task Get_policy_by_id_success()
        {
            // Arrange
            var licenseNumber = "D00027";
            int policyId = 1;

            // Act
            var result = await _controller.GetPolicyById(policyId, licenseNumber);
            var insurancePoliciesResult = (OkObjectResult)result.Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_fakeInsurancePoliciesContext.First(), insurancePoliciesResult.Value);
        }

        [Fact]
        public async Task Post_policy_success()
        {
            // Arrange
            CreatePolicyRequest req = new CreatePolicyRequest
            {
                InsurancePolicy = new InsurancePolicy
                {
                    Id = 4,
                    FirstName = "Walterio",
                    LastName = "Valdez",
                    EffectiveDate = DateTime.Now.AddMonths(2),
                    Address = new Address { City = "Lowell", Country = "USA", State = "Massachusetts", Street = "", ZipCode = "01850" },
                    VehicleDetail = new Vehicle { Id = 4, Model = "Impala", Year = 1967, Manufacturer = "Chevrolet", Name = "Supernatural" }
                }
            };

            // Act
            var result = await _controller.Post(req);


            // Assert
            Assert.NotNull(result);
            Assert.Equal(_fakeInsurancePoliciesContext.LastOrDefault(), req.InsurancePolicy);
        }


        private List<InsurancePolicy> GetFakeInsurancePolicies()
        {
            return new List<InsurancePolicy>()
            {
                new()
                {
                    Id = 1,
                    FirstName="Walterio",
                    LastName="Valdez",
                    EffectiveDate = DateTime.Now.AddMonths(2),
                    LicenseNumber = "D00027",
                    ExpirationDate = DateTime.Now.AddYears(1),
                    Premium = 10.5,
                    VehicleDetailId = 1,
                    VehicleDetail = new()
                    {
                        Id=1,
                        Manufacturer="Chevrolet",
                        Model="Impala",
                        Name="Supernatural",
                        Year=1967
                    },
                    Address = new()
                    {
                        City="New York City",
                        Country="USA",
                        State="New York",
                        Street="350 Fifth Avenue",
                        ZipCode="10118"
                    }
                },
                new()
                {
                    Id = 2,
                    FirstName="Luke",
                    LastName="Duke",
                    EffectiveDate = DateTime.Now,
                    LicenseNumber = "D00054",
                    ExpirationDate = DateTime.Now.AddYears(-1),
                    Premium = 10.5,
                    VehicleDetailId = 2,
                    VehicleDetail = new()
                    {
                        Id=2,
                        Manufacturer="Dodge",
                        Model="Charger",
                        Name="General Lee",
                        Year=1969
                    },
                    Address = new()
                    {
                        City="New York City",
                        Country="USA",
                        State="New York",
                        Street="350 Fifth Avenue",
                        ZipCode="10118"
                    }
                },
                new()
                {
                    Id = 3,
                    FirstName="Eduardo",
                    LastName="Flores",
                    EffectiveDate = DateTime.Now.AddMonths(2),
                    LicenseNumber = "D00027",
                    ExpirationDate = DateTime.Now.AddYears(1),
                    Premium = 10.5,
                    VehicleDetailId = 3,
                    VehicleDetail = new()
                    {
                        Id=3,
                        Manufacturer="Honda",
                        Model="CR-V",
                        Name="CR-v",
                        Year=2017
                    },
                    Address = new()
                    {
                        City="Cancun",
                        Country="MX",
                        State="Quintana Roo",
                        Street="Circuito Almeria",
                        ZipCode="77510"
                    }
                }
            };
        }
    }
}
