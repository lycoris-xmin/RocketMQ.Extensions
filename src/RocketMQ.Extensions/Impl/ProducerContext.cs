namespace RocketMQ.Extensions.Impl
{
    public class ProducerContext<T> : IProducerContext<T> where T : class, IRocketMQProducer
    {
        /// <summary>
        /// 
        /// </summary>
        public T Producer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="implementation"></param>
        public ProducerContext(T implementation)
        {
            Producer = implementation;
        }
    }
}
