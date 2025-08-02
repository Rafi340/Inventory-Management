using Autofac.Extras.Moq;
using AutoMapper;
using Inventory.Application;
using Inventory.Application.Exceptions;
using Inventory.Application.Features.BalanceTransfers.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Test
{
    [ExcludeFromCodeCoverage]
    public class BalanceTransferAddCommandHandlerTests
    {
        private AutoMock _moq;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private Mock<IBalanceTransferRepository> _balanceTransferRepoMock;
        private Mock<IAccountRepository> _accountRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<BalanceTransferAddCommandHandler> _handlerMoq;
        private BalanceTransferAddCommandHandler _handler;
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
            _balanceTransferRepoMock?.Reset();
            _accountRepoMock?.Reset();
        }
        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _balanceTransferRepoMock = new Mock<IBalanceTransferRepository>();
            _accountRepoMock = new Mock<IAccountRepository>();
            _mapperMock = new Mock<IMapper>();
            _handlerMoq = new Mock<BalanceTransferAddCommandHandler>();
            _handler = new BalanceTransferAddCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }
        [Test]
        public async Task AddBalanceTransfer_SenderBalance_AddsBalanceTransfer()
        {
            // Arrange
            var command = new BalanceTransferAddCommand
            {
                SenderAccountId = 1,
                ReceiverAccountId = 2,
                TransferAmount = 100
            };
            var sender = new Account { Id = 1, AccountTypeId= 1, AccountNumber= "2322423", Balance = 500 };
            var receiver = new Account { Id = 2, AccountTypeId = 2, AccountNumber = "22342424", Balance = 50 };
            var balanceTransfer = new BalanceTransfer();

            _accountRepoMock.Setup(x => x.GetByIdAsync(command.SenderAccountId)).ReturnsAsync(sender).Verifiable();
            _accountRepoMock.Setup(x => x.GetByIdAsync(command.ReceiverAccountId)).ReturnsAsync(receiver).Verifiable();
            _accountRepoMock.Setup(x => x.EditAsync(sender)).Verifiable();
            _accountRepoMock.Setup(x => x.EditAsync(receiver)).Verifiable();

            _mapperMock.Setup(x => x.Map<BalanceTransfer>(command)).Returns(balanceTransfer).Verifiable();
            _balanceTransferRepoMock.Setup(x => x.AddAsync(balanceTransfer)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();

            _unitOfWorkMock.SetupGet(x => x.AccountRepository).Returns(_accountRepoMock.Object);
            _unitOfWorkMock.SetupGet(x => x.BalanceTransferRepository).Returns(_balanceTransferRepoMock.Object);

            _handler = new BalanceTransferAddCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            await _handler.Handle(command, CancellationToken.None);


            // Assert
            this.ShouldSatisfyAllConditions(
                () => sender.Balance.ShouldBe(400),
                () => receiver.Balance.ShouldBe(150),
                _accountRepoMock.VerifyAll,
                _balanceTransferRepoMock.VerifyAll,
                _unitOfWorkMock.VerifyAll,
                _mapperMock.VerifyAll
            );
        }

        [Test]
        public void Handle_InsufficientBalance_ThrowsException()
        {
            // Arrange
            var command = new BalanceTransferAddCommand
            {
                SenderAccountId = 1,
                ReceiverAccountId = 2,
                TransferAmount = 300
            };

            var sender = new Account { Id = 1, AccountTypeId = 1, AccountNumber = "2322423", Balance = 100 };

            _accountRepoMock.Setup(x => x.GetByIdAsync(command.SenderAccountId)).ReturnsAsync(sender).Verifiable();
            _unitOfWorkMock.SetupGet(x => x.AccountRepository).Returns(_accountRepoMock.Object);

            // Act & Assert
            Should.Throw<InsufficientBalanceException>(() =>
                _handler.Handle(command, CancellationToken.None)
            );
        }
    }
}
