using Org.Apache.Rocketmq;

namespace RocketMQ.Extensions
{
    public interface IRocketMQConsumer
    {
        Task InvokeAsync(MessageView message);
    }
}
