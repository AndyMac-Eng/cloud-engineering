using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace src;

public class GetCustomer
{
    private readonly ILogger<GetCustomer> _logger;

    public GetCustomer(ILogger<GetCustomer> logger)
    {
        _logger = logger;
    }

    [Function("GetCustomer")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
