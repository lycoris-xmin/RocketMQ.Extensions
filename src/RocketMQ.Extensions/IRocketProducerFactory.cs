using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions
{
    public interface IRocketProducerFactory
    {
        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Producer Create(string name);
    }
}
