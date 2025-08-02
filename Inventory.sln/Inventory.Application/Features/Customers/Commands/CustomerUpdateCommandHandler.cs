using AutoMapper;
using Inventory.Application.Features.Products.Commands;
using Inventory.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Customers.Commands
{
    public class CustomerUpdateCommandHandler : IRequestHandler<CustomerUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAwsUtility _awsUtility;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CustomerUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper,
            IHostingEnvironment hostingEnvironment,
            IAwsUtility awsUtility,
            IConfiguration configuration
            )
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _awsUtility = awsUtility;
            _configuration = configuration;
        }
        public async Task Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            Customer map = _mapper.Map<Customer>(request);
            if (!_applicationUnitOfWork.CustomerRepository.IsCustomerIdDuplicate(map.CustomerId, map.Id))
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
                    var body = new { TableName = "Customers", Key = map.Id };
                    var message = JsonSerializer.Serialize(body);
                    await _awsUtility.SendMessageSQS(message, null, _configuration["Aws:SQSUrl"]);
                }
                await _applicationUnitOfWork.CustomerRepository.EditAsync(map);
                await _applicationUnitOfWork.SaveAsync();

            }
            
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
