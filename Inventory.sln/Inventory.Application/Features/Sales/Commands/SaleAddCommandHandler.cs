using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Inventory.Application.Features.Sales.Commands
{
    public class SaleAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
    IMapper mapper, IConfiguration configuration, IAwsUtility awsUtility) : IRequestHandler<SaleAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IAwsUtility _awsUtility = awsUtility;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(SaleAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.SalesItems.Any()) throw new EmtyItemExceptions();
            var map = _mapper.Map<Sale>(request);
            map.CreatedAt = DateTime.Now;
            map.InvoiceNo = _applicationUnitOfWork.SaleRepository.GenerateInvoiceNo();
            if (!_applicationUnitOfWork.SaleRepository.IsInvoiceDuplicate(map.InvoiceNo))
            {

                var netAmount = request.SalesItems.Sum(t => t.Quantity * t.UnitPrice);
                var vatCalulate = (netAmount * map.Vat) / 100;
                map.NetAmount = netAmount + vatCalulate;
                map.TotalAmount = map.NetAmount - map.Discount;
                map.DueAmount = map.TotalAmount - map.PaidAmount;

                if (map.PaidAmount >= map.TotalAmount)
                {
                    map.Status = 1;
                } else if (map.PaidAmount > 0)
                {
                    map.Status = 2;
                }
                if (map.DueAmount == map.TotalAmount)
                {
                    map.Status = 0;
                }
                foreach (var product in map.SalesItems)
                {
                    var getProduct = _applicationUnitOfWork.ProductRepository.GetById(product.ProductId);
                    getProduct.Quantity = getProduct.Quantity - product.Quantity;
                    await _applicationUnitOfWork.ProductRepository.EditAsync(getProduct);
                }


                await _applicationUnitOfWork.SaleRepository.AddAsync(map);
                await _applicationUnitOfWork.SaveAsync();
                var getCustomerInfo =  await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(map.CustomerId);

                string mailBody = $"<p>Dear <strong>{getCustomerInfo.Name}</strong>,</p>" +
                        $"<p>Your invoice number is <strong>{map.InvoiceNo}</strong>.</p>" +
                         $"<p>Please <a href='/admin/sales/invoice/{map.Id}' target='_blank'>click here</a> to see the details of your invoice.</p>" +
                        $"<p>Thank you,<br/> DevSkill Inventory Management</p>";

                var body = new { EmailTo = getCustomerInfo.Email, ReceiverName = getCustomerInfo.Name,
                    Subject = $"Billing Information: Invoice {map.InvoiceNo}",
                    MailBody = mailBody, Key = map.Id };
                var message = JsonSerializer.Serialize(body);
                await _awsUtility.SendMessageSQS(message, null, _configuration["Aws:SQSEmailUrl"]);

            }
            else
            {
                throw new DuplicateSaleExceptions();
            }
        }
       
    }
}
