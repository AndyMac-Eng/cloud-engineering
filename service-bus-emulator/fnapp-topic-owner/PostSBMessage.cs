using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;

namespace fnapp_topic_owner;

public class PostSBMessage
{
    private readonly ILogger<PostSBMessage> _logger;
    private readonly IConfiguration _config;

    public PostSBMessage(ILogger<PostSBMessage> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    [Function("PostSBMessage")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
            string connectionString = _config["ServiceBusConnection"];
            string topicName = "topic.1";
            string messageBody = "Hello from Azure Function!";

            try
            {
                var client = new ServiceBusClient(connectionString);
                var sender = client.CreateSender(topicName);
                var message = new ServiceBusMessage(messageBody);
                sender.SendMessageAsync(message).GetAwaiter().GetResult();
                sender.DisposeAsync().GetAwaiter().GetResult();
                client.DisposeAsync().GetAwaiter().GetResult();
                _logger.LogInformation($"Message sent to topic: {topicName}");
                return new OkObjectResult($"Message sent to topic: {topicName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to Service Bus topic.");
                return new ObjectResult($"Error: {ex.Message}") { StatusCode = 500 };
            }
    }
}
