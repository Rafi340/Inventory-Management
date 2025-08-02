using Amazon.SQS;
using Amazon.SQS.Model;
using Inventory.Application;
using Inventory.Application.Features.Customers.Queries;
using Inventory.Application.Features.Products.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Utilities;
using Inventory.Infrastructure.Utilities;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static Inventory.Worker.Worker;
namespace Inventory.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _imagePath;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IImageResizerUtility _imageResizerUtility;
        private readonly IEmailUtility _emailUtility;
        private readonly IAwsUtility _awsUtility;
        public Worker(ILogger<Worker> logger, IMediator mediator, IConfiguration configuration, IImageResizerUtility imageResizerUtility, IAwsUtility awsUtility, IEmailUtility emailUtility)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
            _imagePath = _configuration["File:ImagePath"];
            _imageResizerUtility = imageResizerUtility;
            _awsUtility = awsUtility;
            _emailUtility = emailUtility;
        }
        public class Response
        {
            public string TableName { get; set; }
            public string Key { get; set; }
        }
        public class EmailResponse
        {
            public string EmailTo { get; set; }
            public string ReceiverName { get; set; }
            public string MailBody { get; set; }
            public string Subject { get; set; }
            public string Key { get; set; }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var receiveMessageResponse = await _awsUtility.ReceiveMessageResponseSQS(_configuration["Aws:QueueNameForFile"]);
                if (receiveMessageResponse.Messages != null && receiveMessageResponse.Messages.Count > 0)
                {
                    var body = JsonSerializer.Deserialize<Response>(receiveMessageResponse?.Messages[0]?.Body);
                    string imageFileName = "";
                    if (body.TableName == "Customers")
                    {
                        var CustomerId = new GetCustomerByIdQuery
                        {
                            CustomerId = Guid.Parse(body.Key)
                        };
                       var customer = await _mediator.Send(CustomerId);
                       imageFileName = customer?.ImageUrl;
                    }
                    if(body.TableName == "Product")
                    {
                        var productId = new GetProductByIdQuery
                        {
                            Id = Guid.Parse(body.Key)
                        };
                        var product = await _mediator.Send(productId);
                        imageFileName = product?.ImageUrl;
                    }
                        
                        var fullPath = Path.Combine(_imagePath, imageFileName);
                        if(File.Exists(fullPath))
                        {
                           await _imageResizerUtility.ResizeImageAsync(fullPath);
                           var fileToBucket = await _awsUtility.UploadFileAsync(imageFileName, fullPath);
                           if (fileToBucket)
                           {
                                var deleteMessageReponse = await _awsUtility.DeleteMessageResponseSQS(receiveMessageResponse.Messages[0].ReceiptHandle, _configuration["Aws:QueueNameForFile"]);
                                if (deleteMessageReponse) File.Delete(fullPath);
                           }
                        }
                        //await _awsUtility.DeleteMessageResponseSQS(receiveMessageResponse.Messages[0].ReceiptHandle, _configuration["Aws:QueueNameForFile"]);
                    
                }
                var receiveMessageResponseForEmail = await _awsUtility.ReceiveMessageResponseSQS(_configuration["Aws:QueueNameForEmail"]);
                if(receiveMessageResponseForEmail != null && receiveMessageResponseForEmail.Messages.Count > 0)
                {
                    var body = JsonSerializer.Deserialize<EmailResponse>(receiveMessageResponseForEmail?.Messages[0]?.Body);
                    _emailUtility.SendEmail(body.EmailTo, body.ReceiverName, body.Subject, body.MailBody);
                    await _awsUtility.DeleteMessageResponseSQS(receiveMessageResponseForEmail.Messages[0].ReceiptHandle, _configuration["Aws:QueueNameForEmail"]);
                }
               
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
