using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions
{
    public sealed class BaseProducer
    {
        /// <summary>
        /// 
        /// </summary>
        internal Producer Producer { get; }

        /// <summary>
        /// 
        /// </summary>
        internal RocketMqProducerOptions Options { get; }

        public BaseProducer(Producer producer, RocketMqProducerOptions options)
        {
            Producer = producer;
            Options = options;
        }

        /// <summary>
        /// 普通消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishAsync<T>(string topic, string tag, T message) where T : class
            => this.PublishAsync(topic, tag, Newtonsoft.Json.JsonConvert.SerializeObject(message));

        /// <summary>
        /// 普通消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishAsync(string topic, string tag, string message)
        {
            if (!this.Options.Topics.Contains(topic))
                throw new Exception($"the current topic:{topic} is not included in the producer configuration:{string.Join(",", this.Options.Topics)}");

            return this.Producer.PublishAsync(topic, tag, message);
        }

        /// <summary>
        /// 顺序消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishFifoAsync<T>(string topic, string tag, string group, string message) where T : class
            => this.PublishFifoAsync(topic, tag, group, Newtonsoft.Json.JsonConvert.SerializeObject(message));

        /// <summary>
        /// 顺序消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishFifoAsync(string topic, string tag, string group, string message)
        {
            if (!this.Options.Topics.Contains(topic))
                throw new Exception($"the current topic:{topic} is not included in the producer configuration:{string.Join(",", this.Options.Topics)}");

            return this.Producer.PublishFifoAsync(topic, tag, group, message);
        }

        /// <summary>
        /// 延迟消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishDelayAsync<T>(string topic, string tag, T message, TimeSpan timespan) where T : class
            => this.PublishDelayAsync(topic, tag, Newtonsoft.Json.JsonConvert.SerializeObject(message), timespan);

        /// <summary>
        /// 延迟消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishDelayAsync<T>(string topic, string tag, T message, DateTime delayTime) where T : class
            => this.PublishDelayAsync(topic, tag, Newtonsoft.Json.JsonConvert.SerializeObject(message), delayTime);

        /// <summary>
        /// 延迟消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishDelayAsync(string topic, string tag, string message, TimeSpan timespan)
            => this.PublishDelayAsync(topic, tag, message, timespan);

        /// <summary>
        /// 延迟消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public Task<ISendReceipt> PublishDelayAsync(string topic, string tag, string message, DateTime delayTime)
        {
            if (!this.Options.Topics.Contains(topic))
                throw new Exception($"the current topic:{topic} is not included in the producer configuration:{string.Join(",", this.Options.Topics)}");

            return this.Producer.PublishDelayAsync(topic, tag, message, delayTime);
        }
    }
}
