using Microsoft.Extensions.DependencyInjection;
using Org.Apache.Rocketmq;
using System.Text;

namespace Lycoris.RocketMQ.Extensions
{
    public static class RocketMqBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static RocketMqBuilder AddRocketMq(this IServiceCollection services, Action<RocketMqBuilder> configure)
        {
            var builder = new RocketMqBuilder(services);

            configure.Invoke(builder);

            foreach (var options in builder.ProducerOptions)
            {
                options.Value.AccessKey = builder.AccessKey;
                options.Value.SecretKey = builder.SecretKey;
                options.Value.Endpoints = builder.Endpoints;

                RocketOptionsStore.AddOrUpdateProducerOptions(options.Key, options.Value);
            }

            return builder;
        }

        /// <summary>
        /// 普通消息
        /// </summary>
        /// <param name="producer"></param>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task<ISendReceipt> PublishAsync(this Producer producer, string topic, string tag, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            var body = new Message.Builder()
                                 .SetTopic(topic)
                                 .SetBody(bytes)
                                 .SetTag(tag)
                                 .SetKeys($"{topic}-{Guid.NewGuid():N}")
                                 .Build();

            return producer.Send(body);
        }

        /// <summary>
        /// 顺序消息
        /// </summary>
        /// <param name="producer"></param>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task<ISendReceipt> PublishFifoAsync(this Producer producer, string topic, string tag, string group, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            var body = new Message.Builder()
                                 .SetTopic(topic)
                                 .SetBody(bytes)
                                 .SetTag(tag)
                                 .SetKeys($"{topic}-{Guid.NewGuid():N}")
                                 .SetMessageGroup(group)
                                 .Build();

            return producer.Send(body);
        }

        /// <summary>
        /// 延迟消息
        /// </summary>
        /// <param name="producer"></param>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>
        public static Task<ISendReceipt> PublishDelayAsync(this Producer producer, string topic, string tag, string message, TimeSpan timespan)
            => producer.PublishDelayAsync(topic, tag, message, DateTime.Now.AddSeconds(timespan.TotalSeconds));

        /// <summary>
        /// 延迟消息
        /// </summary>
        /// <param name="producer"></param>
        /// <param name="topic"></param>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public static Task<ISendReceipt> PublishDelayAsync(this Producer producer, string topic, string tag, string message, DateTime delayTime)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            var body = new Message.Builder()
                                 .SetTopic(topic)
                                 .SetBody(bytes)
                                 .SetTag(tag)
                                 .SetKeys($"{topic}-{Guid.NewGuid():N}")
                                 .SetDeliveryTimestamp(delayTime)
                                 .Build();

            return producer.Send(body);
        }
    }
}
