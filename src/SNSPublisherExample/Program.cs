using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SNSPublisherExample;
using System.Text.Json;

string sqsQueueName = "users";
var snsClient = new AmazonSimpleNotificationServiceClient();
var usersTopic = await snsClient.FindTopicAsync(sqsQueueName);

Guid newUserId = Guid.NewGuid();
UserCreatedMessage userCreatedMessage = new(newUserId, "juanmg", "Juan M.", "juanmg@domain.com");
var userCreatedMessagePublishRequest = new PublishRequest
{
    TopicArn = usersTopic.TopicArn,
    Message = JsonSerializer.Serialize(userCreatedMessage),
    MessageAttributes = new()
    {
        {
            "MessageType",
            new() { DataType = "String", StringValue = typeof(UserCreatedMessage).Name }
        }
    }
};

Console.WriteLine("Publishing UserCreatedMessage...");

var response = await snsClient.PublishAsync(userCreatedMessagePublishRequest);

Console.WriteLine($"UserCreatedMessage published. Response StatusCode: {response.HttpStatusCode} - RequestId: {response.ResponseMetadata.RequestId}");

UserDeletedMessage userDeletedMessage = new(newUserId);
var userDeletedMessageRequest = new PublishRequest
{
    TopicArn = usersTopic.TopicArn,
    Message = JsonSerializer.Serialize(userDeletedMessage),
    MessageAttributes = new()
    {
        {
            "MessageType",
            new() { DataType = "String", StringValue = typeof(UserDeletedMessage).Name }
        }
    }
};

Console.WriteLine("Publishing UserCreatedMessage...");

response = await snsClient.PublishAsync(userDeletedMessageRequest);

Console.WriteLine($"UserCreatedMessage published. Response StatusCode: {response.HttpStatusCode} - RequestId: {response.ResponseMetadata.RequestId}");
