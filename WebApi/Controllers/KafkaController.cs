using Application.Common.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KafkaController(IKafkaProducer producer) : ControllerBase
{
    [HttpPost("test")]
    public async Task<IActionResult> Send(string message, CancellationToken cancellationToken)
    {
       // await producer.ProduceAsync("users-topic", message, cancellationToken);
        return Ok();
    }
}
