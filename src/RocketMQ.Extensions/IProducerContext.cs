namespace RocketMQ.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProducerContext<T> where T : class, IRocketMQProducer
    {
        T Producer { get; }
    }
}
