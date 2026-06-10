using Application.Common.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KafkaController(IKafkaProducer producer) : ControllerBase
{
    [HttpPost("test")]
    public async Task<IActionResult> Send(CancellationToken cancellationToken)
    {
        await producer.ProduceAsync("test-topic", "test-message", cancellationToken);
        return Ok();
    }
}
