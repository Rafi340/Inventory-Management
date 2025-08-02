using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Application.Features.Sales.Commands;
using Inventory.Application;
using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Inventory.Domain.Dtos;
using Autofac.Extras.Moq;

namespace Inventory.Test
{
    public class SaleAddCommandHandlerTests
    {
        private AutoMock _moq;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<ISaleRepository> _saleRepoMock;
        private Mock<IProductRepository> _productRepoMock;
        private Mock<ICustomerRepository> _customerRepoMock;
        private Mock<IAwsUtility> _awsUtilityMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IConfiguration> _configMock;
        private SaleAddCommandHandler _handler;
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
            _saleRepoMock?.Reset();
            _productRepoMock?.Reset();
            _productRepoMock?.Reset();
            _awsUtilityMock?.Reset();
            _customerRepoMock?.Reset();
        }

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _saleRepoMock = new Mock<ISaleRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _awsUtilityMock = new Mock<IAwsUtility>();
            _mapperMock = new Mock<IMapper>();
            _configMock = new Mock<IConfiguration>();

            _unitOfWorkMock.SetupGet(x => x.SaleRepository).Returns(_saleRepoMock.Object);
            _unitOfWorkMock.SetupGet(x => x.ProductRepository).Returns(_productRepoMock.Object);
            _unitOfWorkMock.SetupGet(x => x.CustomerRepository).Returns(_customerRepoMock.Object);

            _handler = new SaleAddCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _configMock.Object,
                _awsUtilityMock.Object
            );
        }

        [Test]
        public async Task Handle_ValidSale_AddsSaleAndSendsEmail()
        {
            // Arrange
            var command = new SaleAddCommand
            {
                CustomerId = new Guid("A1E9CEDA-D99B-42E0-04CE-08DDC7B465F1"),
                Discount = 10,
                SaleDate = DateTime.UtcNow,
                PaidAmount = 100,
                SalesTypeId = 1,
                AcountTypeId = 1,

                Vat = 5,
                SalesItems = new List<SaleItemAddDto>
                {
                    new SaleItemAddDto { ProductId = new Guid("062BD6C4-B3FB-48BF-6BA0-08DDBCDDFDF5"), Quantity = 2, UnitPrice = 50 }
                }
            };

            var mappedSale = new Sale
            {
                CustomerId = new Guid("A1E9CEDA-D99B-42E0-04CE-08DDC7B465F1"),
                Discount = 10,
                SaleDate = DateTime.UtcNow,
                PaidAmount = 100,
                SalesTypeId = 1,
                AcountTypeId = 1,
                InvoiceNo= "INV_00001",
                Vat = 5,
                SalesItems = new List<SaleItems>
                {
                    new SaleItems { ProductId = new Guid("062BD6C4-B3FB-48BF-6BA0-08DDBCDDFDF5"), Quantity = 2, UnitPrice = 50 }
                }
            };

            var product = new Product { Id = new Guid("062BD6C4-B3FB-48BF-6BA0-08DDBCDDFDF5"), Quantity = 10 };
            var customer = new Customer { Id = new Guid("A1E9CEDA-D99B-42E0-04CE-08DDC7B465F1"), CustomerId= "CUS_000001", Name = "Rafi Samnan", Email = "Rafi Samnan" , Mobile = "3433453"};

            _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(mappedSale);
            _saleRepoMock.Setup(s => s.GenerateInvoiceNo()).Returns("INV_00001");
            _saleRepoMock.Setup(s => s.IsInvoiceDuplicate("INV_00001", null)).Returns(false);
            _productRepoMock.Setup(p => p.GetById(new Guid("062BD6C4-B3FB-48BF-6BA0-08DDBCDDFDF5"))).Returns(product);
            _configMock.Setup(c => c["Aws:SQSEmailUrl"]).Returns("https://fake-sqs-url");
            _customerRepoMock.Setup(c => c.GetByIdAsync(new Guid("A1E9CEDA-D99B-42E0-04CE-08DDC7B465F1"))).ReturnsAsync(customer);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            mappedSale.InvoiceNo.ShouldBe("INV_00001");
            mappedSale.TotalAmount.ShouldBeGreaterThan(0);
            mappedSale.NetAmount.ShouldBeGreaterThan(0);
            mappedSale.Status.ShouldBeInRange(0, 2);

            _productRepoMock.Verify(p => p.EditAsync(It.Is<Product>(prod => prod.Id == new Guid("062BD6C4-B3FB-48BF-6BA0-08DDBCDDFDF5") && prod.Quantity == 8)), Times.Once);
            _saleRepoMock.Verify(s => s.AddAsync(mappedSale), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
            _awsUtilityMock.Verify(a =>
                a.SendMessageSQS(It.Is<string>(msg => msg.Contains(customer.Email)), null, "https://fake-sqs-url"), Times.Once);
        }

        [Test]
        public void Handle_EmptySalesItems_ThrowsEmtyItemExceptions()
        {
            var command = new SaleAddCommand
            {
                CustomerId = new Guid("A1E9CEDA-D99B-42E0-04CE-08DDC7B465F1"),
                Discount = 10,
                SaleDate = DateTime.UtcNow,
                PaidAmount = 100,
                SalesTypeId = 1,
                AcountTypeId = 1,
                Vat = 5,
                SalesItems = new List<SaleItemAddDto>()
            };

            Should.ThrowAsync<EmtyItemExceptions>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_DuplicateInvoice_ThrowsDuplicateSaleExceptions()
        {
            var command = new SaleAddCommand
            {
                CustomerId = new Guid("A1E9CEDA-D99B-42E0-04CE-08DDC7B465F1"),
                Discount = 10,
                SaleDate = DateTime.UtcNow,
                PaidAmount = 100,
                SalesTypeId = 1,
                AcountTypeId = 1,
                Vat = 5,
                SalesItems = new List<SaleItemAddDto>
                {
                    new SaleItemAddDto { ProductId = new Guid("062BD6C4-B3FB-48BF-6BA0-08DDBCDDFDF5"), Quantity = 2, UnitPrice = 50 }
                }
            };

            var mappedSale = new Sale
            {
                CustomerId = new Guid("A1E9CEDA-D99B-42E0-04CE-08DDC7B465F1"),
                Discount = 10,
                InvoiceNo = "INV_00001",
                SaleDate = DateTime.UtcNow,
                PaidAmount = 100,
                SalesTypeId = 1,
                AcountTypeId = 1,
                Vat = 5,
                SalesItems = new List<SaleItems>
                {
                    new SaleItems { Id= new Guid("CD7BF62D-7ECB-4C21-23B9-08DDC9DF35F2"), SalesId= new Guid("76F2DC74-B036-43C9-D06B-08DDC9DF35EC") ,ProductId = new Guid("062BD6C4-B3FB-48BF-6BA0-08DDBCDDFDF5"), Quantity = 2, UnitPrice = 50 }
                }
                
            };

            _mapperMock.Setup(m => m.Map<Sale>(command)).Returns(mappedSale);
            _saleRepoMock.Setup(s => s.GenerateInvoiceNo()).Returns("INV001");
            _saleRepoMock.Setup(s => s.IsInvoiceDuplicate("INV_00001",null)).Returns(true);

            Should.ThrowAsync<DuplicateSaleExceptions>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
