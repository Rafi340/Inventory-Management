using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.BalanceTransfers.Commands
{
    public class BalanceTransferAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper) : IRequestHandler<BalanceTransferAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(BalanceTransferAddCommand request, CancellationToken cancellationToken)
        {
           
                var getSenderAccount = await _applicationUnitOfWork.AccountRepository.GetByIdAsync(request.SenderAccountId);
                if (getSenderAccount.Balance >= request.TransferAmount)
                {

                    getSenderAccount.Balance -= request.TransferAmount;
                    await _applicationUnitOfWork.AccountRepository.EditAsync(getSenderAccount);

                    var getReceiverAccount = await _applicationUnitOfWork.AccountRepository.GetByIdAsync(request.ReceiverAccountId);
                    getReceiverAccount.Balance += request.TransferAmount;
                    await _applicationUnitOfWork.AccountRepository.EditAsync(getReceiverAccount);

                    var map = _mapper.Map<BalanceTransfer>(request);
                    map.CreatedAt = DateTime.Now;
                    await _applicationUnitOfWork.BalanceTransferRepository.AddAsync(map);
                    await _applicationUnitOfWork.SaveAsync();
                }
                else
                    throw new InsufficientBalanceException();
        }
    }
}
