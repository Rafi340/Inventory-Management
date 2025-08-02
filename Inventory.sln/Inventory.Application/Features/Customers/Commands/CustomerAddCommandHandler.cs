using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Inventory.Application.Features.Customers.Commands
{
    public class CustomerAddCommandHandler : IRequestHandler<CustomerAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAwsUtility _awsUtility;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CustomerAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper,
            IHostingEnvironment hostingEnvironment, IAwsUtility awsUtility, IConfiguration configuration)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _awsUtility = awsUtility;
            _configuration = configuration;
        }
        public async Task Handle(CustomerAddCommand request, CancellationToken cancellationToken)
        {

            Customer map = _mapper.Map<Customer>(request);
            map.CustomerId = _applicationUnitOfWork.CustomerRepository.GenerateCustomerId();
            if (!_applicationUnitOfWork.CustomerRepository.IsCustomerIdDuplicate(map.CustomerId))
            {
                if (request.ImageFile != null)
                {
                    var uniqueFileName = GetUniqueFileName(request.ImageFile.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "imageupload");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    request.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    map.ImageUrl = uniqueFileName;
                }
                await _applicationUnitOfWork.CustomerRepository.AddAsync(map);
                await _applicationUnitOfWork.SaveAsync();
                if (request.ImageFile != null)
                {
                    var body = new { TableName = "Customers", Key = map.Id };
                    var message = JsonSerializer.Serialize(body);
                    await _awsUtility.SendMessageSQS(message, null, _configuration["Aws:SQSUrl"]);
                }
                
            }
            else
                throw new DuplicateCustomerIdExceptions();


        }

        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString()
                      + Path.GetExtension(fileName);

        }
       


    }
}
