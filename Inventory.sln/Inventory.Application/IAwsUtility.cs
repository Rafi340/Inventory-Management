using Amazon.SQS.Model;
using Amazon.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application
{
    public interface IAwsUtility
    {
        Task<SendMessageResponse> SendMessageSQS(string messageBody,   Dictionary<string, MessageAttributeValue> messageAttributes, string SqsURL);
        Task<bool> UploadFileAsync(string objectName, string filePath);
        Task<ReceiveMessageResponse> ReceiveMessageResponseSQS(string QueueName);
        Task<bool> DeleteMessageResponseSQS(string ReceiptHandle, string QueueName);
    }
}
