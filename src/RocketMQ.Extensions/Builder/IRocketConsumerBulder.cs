namespace Lycoris.RocketMQ.Extensions.Builder
{
    public interface IRocketConsumerBulder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRocketConsumerBulder AddSimpleConsumer<T>() where T : class, IRocketConsumer;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRocketConsumerBulder AddPushConsumer<T>() where T : class, IRocketConsumer;
    }
}
