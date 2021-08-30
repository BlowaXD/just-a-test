using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public sealed class GenericMockedMessagePublisher<T> : IMessagePublisher<T> where T : IMessage
{
    private readonly ILogger<GenericMockedMessagePublisher<T>> _logger;

    public GenericMockedMessagePublisher(ILogger<GenericMockedMessagePublisher<T>> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync(T message)
    {
        // fake message publisher
        _logger.LogInformation($"MessageSent to broker: {typeof(T)}");
    }
}