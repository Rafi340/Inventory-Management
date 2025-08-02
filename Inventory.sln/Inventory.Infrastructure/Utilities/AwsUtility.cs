using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure;
using Inventory.Application;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Utilities
{
    public class AwsUtility : IAwsUtility
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonSQS client;
        private readonly IAmazonS3 client3;
        private readonly RegionEndpoint ServiceRegion;
        public AwsUtility(IConfiguration configuration) 
        {

            _configuration = configuration;
            ServiceRegion = RegionEndpoint.GetBySystemName(configuration["AWS:Region"] ?? "us-east-1"); ;
            client = new AmazonSQSClient(ServiceRegion);
            client3 = new AmazonS3Client(ServiceRegion);
        }

        public async Task<bool> DeleteMessageResponseSQS(string ReceiptHandle,string QueueName)
        {
            try
            {
                var response = await client.GetQueueUrlAsync(QueueName);
                var deleteMessageRequest = new DeleteMessageRequest
                {
                    QueueUrl = response.QueueUrl,
                    ReceiptHandle = ReceiptHandle,
                };
                await client.DeleteMessageAsync(deleteMessageRequest);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public async Task<ReceiveMessageResponse> ReceiveMessageResponseSQS(string QueueName)
        {
            try
            {
                var response = await client.GetQueueUrlAsync(QueueName);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var receiveMessageRequest = new ReceiveMessageRequest
                    {
                        AttributeNames = new List<string> { "SentTimestamp" },
                        MaxNumberOfMessages = 1,
                        MessageAttributeNames = new List<string> { "All" },
                        QueueUrl = response.QueueUrl,
                        VisibilityTimeout = 1,
                        WaitTimeSeconds = 10,
                    };
                    var receiveResponse = await client.ReceiveMessageAsync(receiveMessageRequest);
                    receiveResponse.Messages ??= new List<Message>();
                    return receiveResponse;

                }
                
            }
            catch (QueueDoesNotExistException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"The queue {_configuration["Aws:QueueName"]} was not found.");
            }
            return new ReceiveMessageResponse
            {
                Messages = new List<Message>()
            };

        }

        public async Task<SendMessageResponse> SendMessageSQS( string messageBody, Dictionary<string, MessageAttributeValue> messageAttributes, string SqsURL)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                DelaySeconds = 10,
                MessageAttributes = messageAttributes,
                MessageBody = messageBody,
                QueueUrl = SqsURL,
            };

            var response = await client.SendMessageAsync(sendMessageRequest);

            return response;
        }

        public async Task<bool> UploadFileAsync(string objectName, string filePath)
        {
            try
            {
                var request = new PutObjectRequest
                {
                     BucketName = _configuration["Aws:BucketName"],
                    Key = objectName,
                    FilePath = filePath,
                };
                await client3.PutObjectAsync(request);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
            
        }
    }
}
