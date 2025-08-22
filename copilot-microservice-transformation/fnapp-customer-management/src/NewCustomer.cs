using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace src;

public class NewCustomer
{
    private readonly ILogger<NewCustomer> _logger;

    public NewCustomer(ILogger<NewCustomer> logger)
    {
        _logger = logger;
    }

    [Function("NewCustomer")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req, Customer customer)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        using (CosmosClient client = new(
            accountEndpoint: "https://localhost:8085/",
            authKeyOrResourceToken: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
        ))
        {
            Database database = await client.CreateDatabaseIfNotExistsAsync(
                id: "mybizdb",
                throughput: 400
            );

            Container container = await database.CreateContainerIfNotExistsAsync(
                id: "customers",
                partitionKeyPath: "/id"
            );

            await container.CreateItemAsync(customer, new PartitionKey(customer.Id));
        }

        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
