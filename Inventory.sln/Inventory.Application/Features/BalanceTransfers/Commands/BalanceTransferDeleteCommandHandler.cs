using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.BalanceTransfers.Commands
{
    public class BalanceTransferDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<BalanceTransferDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(BalanceTransferDeleteCommand request, CancellationToken cancellationToken)
        {
            var getTransfer = await _applicationUnitOfWork.BalanceTransferRepository.GetByIdAsync(request.Id);

            var getToAccount = await _applicationUnitOfWork.AccountRepository.GetByIdAsync(getTransfer.SenderAccountId);
            getToAccount.Balance += getTransfer.TransferAmount;
            await _applicationUnitOfWork.AccountRepository.EditAsync(getToAccount);

            var getFromAccount = await _applicationUnitOfWork.AccountRepository.GetByIdAsync(getTransfer.ReceiverAccountId);
            getFromAccount.Balance -= getTransfer.TransferAmount;
            await _applicationUnitOfWork.AccountRepository.EditAsync(getFromAccount);

            await _applicationUnitOfWork.BalanceTransferRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
