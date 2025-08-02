using Autofac.Extras.Moq;
using AutoMapper;
using Inventory.Application;
using Inventory.Application.Exceptions;
using Inventory.Application.Features.Customers.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using System.Net;
using Shouldly;

namespace Inventory.Test
{
    [ExcludeFromCodeCoverage]
    public class CustomerAddCommandHandlerTests
    {
        private AutoMock _moq;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ICustomerRepository> _customerRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IHostingEnvironment> _envMock;
        private Mock<IAwsUtility> _awsUtilityMock;
        private Mock<IConfiguration> _configMock;
        private CustomerAddCommandHandler _handler;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _moq = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _moq?.Dispose();
        }
        [TearDown]
        public void Teardown()
        {
            _unitOfWorkMock?.Reset();
            _customerRepoMock?.Reset();
            _awsUtilityMock?.Reset();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();
            _envMock = new Mock<IHostingEnvironment>();
            _awsUtilityMock = new Mock<IAwsUtility>();
            _configMock = new Mock<IConfiguration>();

            _unitOfWorkMock.SetupGet(u => u.CustomerRepository)
                           .Returns(_customerRepoMock.Object);

            _handler = new CustomerAddCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _envMock.Object,
                _awsUtilityMock.Object,
                _configMock.Object
            );
        }


        [Test]
        public void Handle_DuplicateCustomerId_ThrowsException()
        {
            // Arrange
            var command = new CustomerAddCommand {
            Name="Rafi Samnan",
            CompanyName = "INfi sysy",
            Mobile="2334345435",
             Address = "Uttara",
             OpeningBalance=23323,
             Status = 1
            };

            var mappedCustomer = new Customer
            {
                Id = new Guid(),
                CustomerId= "CUS_00001",
                Name = "Rafi Samnan",
                CompanyName = "INfi sysy",
                Mobile = "2334345435",
                Address = "Uttara",
                OpeningBalance = 23323,
                Status = 1
            };

            _mapperMock.Setup(m => m.Map<Customer>(command)).Returns(mappedCustomer);

            _customerRepoMock.Setup(r => r.GenerateCustomerId()).Returns("CUS_00001");
            _customerRepoMock.Setup(r => r.IsCustomerIdDuplicate("CUS_00001", null)).Returns(true);

            // Act & Assert
            Should.Throw<DuplicateCustomerIdExceptions>(() => _handler.Handle(command, CancellationToken.None));
        }

        private IFormFile CreateMockFormFile(string fileName)
        {
            var bytes = Encoding.UTF8.GetBytes("Fake image content");
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", fileName);
        }
    }
}
