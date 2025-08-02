using AutoMapper;
using Inventory.Application.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Utilities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Products.Commands
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IFileHelperUtility _fileHelperUtility;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAwsUtility _awsUtility;
        public ProductUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, 
            IMapper mapper,
            IFileHelperUtility fileHelperUtility,
            IHostingEnvironment hostEnvironment,
            IAwsUtility awsUtility,
            IConfiguration configuration
            )
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
            _fileHelperUtility = fileHelperUtility;
            _hostingEnvironment = hostEnvironment;
            _awsUtility = awsUtility;
            _configuration = configuration;
        }
        public async Task Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var map = _mapper.Map<Product>(request);
            if (!_applicationUnitOfWork.ProductRepository.IProductCodeDuplicate(map.ProductCode, map.Id))
            {
                if (request.ImageFile != null)
                {
                    var uniqueFileName = _fileHelperUtility.GetUniqueFileName(request.ImageFile.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "imageupload");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    request.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    map.ImageUrl = uniqueFileName;
                }
                await _applicationUnitOfWork.ProductRepository.EditAsync(map);
                await _applicationUnitOfWork.SaveAsync();
                if (request.ImageFile != null)
                {
                    var body = new { TableName = "Product", Key = map.Id };
                    var message = JsonSerializer.Serialize(body);
                    await _awsUtility.SendMessageSQS(message, null, _configuration["Aws:SQSUrl"]);
                }
            }
            else
                throw new DuplicateProductCodeExceptions();

        }
    }
    
}
