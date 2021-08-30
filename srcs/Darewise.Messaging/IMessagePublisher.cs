using System.Threading.Tasks;

public interface IMessagePublisher<in T> where T : IMessage
{
    Task PublishAsync(T message);
}