using Microsoft.Extensions.DependencyInjection;
using Org.Apache.Rocketmq;
using System.Diagnostics.CodeAnalysis;

namespace RocketMQ.Extensions.Impl
{
    public class BaseRocketMQProducer : IRocketMQProducer
    {
        protected readonly ClientConfig _config;

        public BaseRocketMQProducer(IServiceProvider serviceProvider)
        {
            _config = serviceProvider.GetRequiredService<ClientConfig>();
        }

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishAsync(string topic) => this.PublishAsync(topic, MessageBuilder(topic));

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishAsync(string topic, string body) => this.PublishAsync(topic, MessageBuilder(topic, body: body));

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <param name="tag">标志(路由)</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishAsync(string topic, string tag, string body) => this.PublishAsync(topic, MessageBuilder(topic, tag: tag, body: body));

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="messageId">消息Id</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishAsync(string topic, string tag, string body, string messageId) => this.PublishAsync(topic, MessageBuilder(topic, tag, body, messageId));

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishFifoAsync(string group, string topic) => this.PublishAsync(topic, FifoMessageBuilder(group, topic));

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishFifoAsync(string group, string topic, string body) => this.PublishAsync(topic, FifoMessageBuilder(group, topic, body: body));

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishFifoAsync(string group, string topic, string tag, string body) => this.PublishAsync(topic, FifoMessageBuilder(group, topic, tag, body));

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishFifoAsync(string group, string topic, string tag, string body, string messageId) => this.PublishAsync(topic, FifoMessageBuilder(group, topic, tag, body, messageId));

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishDelayAsync(string topic, TimeSpan delay) => this.PublishAsync(topic, DelayMessageBuilder(topic, delay));

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishDelayAsync(string topic, string body, TimeSpan delay) => this.PublishAsync(topic, DelayMessageBuilder(topic, delay, body: body));

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishDelayAsync(string topic, string tag, string body, TimeSpan delay) => this.PublishAsync(topic, DelayMessageBuilder(topic, delay, tag, body));

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <param name="messageId">消息Id</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public virtual Task<ISendReceipt?> PublishDelayAsync(string topic, string tag, string body, string messageId, TimeSpan delay) => this.PublishAsync(topic, DelayMessageBuilder(topic, delay, tag, body, messageId));

        /// <summary>
        /// 日志记录-发送前触发
        /// </summary>
        /// <param name="message"></param>
        protected virtual Task LoggerBeforePublishAsync(Org.Apache.Rocketmq.Message message)
        {
            //
            return Task.CompletedTask;
        }

        /// <summary>
        /// 日志记录-发送后触发
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual Task LoggerAfterPublishAsync(Org.Apache.Rocketmq.Message message, Exception? ex = null)
        {
            //
            return Task.CompletedTask;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<ISendReceipt?> PublishAsync(string topic, Org.Apache.Rocketmq.Message message)
        {
            Producer? producer = null;

            try
            {
                await this.LoggerBeforePublishAsync(message);

                producer = await new Producer.Builder().SetTopics(topic).SetClientConfig(_config).Build();

                var result = await producer.Send(message);

                await this.LoggerAfterPublishAsync(message);

                return result;
            }
            catch (Exception ex)
            {
                await this.LoggerAfterPublishAsync(message, ex);
                throw;
            }
            finally
            {
                if (producer != null)
                    await producer.DisposeAsync();
            }
        }

        /// <summary>
        /// 普通消息构建
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <param name="messageId">消息Id</param>
        /// <returns></returns>
        private static Org.Apache.Rocketmq.Message MessageBuilder([NotNull] string topic, string? tag = null, string? body = null, string? messageId = null)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(body ?? "");

            var builder = new Org.Apache.Rocketmq.Message.Builder();

            builder.SetTopic(topic);

            builder.SetBody(bytes);

            if (!string.IsNullOrEmpty(tag))
                builder.SetTag(tag);

            if (string.IsNullOrEmpty(messageId))
                messageId = Guid.NewGuid().ToString("N");

            builder.SetKeys(messageId);

            return builder.Build();
        }

        /// <summary>
        /// 顺序消息构建
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <param name="messageId">消息Id</param>
        /// <returns></returns>
        private static Org.Apache.Rocketmq.Message FifoMessageBuilder([NotNull] string group, [NotNull] string topic, string? tag = null, string? body = null, string? messageId = null)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(body ?? "");

            var builder = new Org.Apache.Rocketmq.Message.Builder();

            builder.SetTopic(topic);

            builder.SetBody(bytes);

            builder.SetMessageGroup(group);

            if (!string.IsNullOrEmpty(tag))
                builder.SetTag(tag);

            if (string.IsNullOrEmpty(messageId))
                messageId = Guid.NewGuid().ToString("N");

            builder.SetKeys(messageId);

            return builder.Build();
        }

        /// <summary>
        /// 延迟消息构建
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <param name="messageId">消息Id</param>
        /// <returns></returns>
        private static Org.Apache.Rocketmq.Message DelayMessageBuilder([NotNull] string topic, [NotNull] TimeSpan delay, string? tag = null, string? body = null, string? messageId = null)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(body ?? "");

            var builder = new Org.Apache.Rocketmq.Message.Builder();

            builder.SetTopic(topic);

            builder.SetBody(bytes);

            if (!string.IsNullOrEmpty(tag))
                builder.SetTag(tag);

            if (string.IsNullOrEmpty(messageId))
                messageId = Guid.NewGuid().ToString("N");

            builder.SetKeys(messageId);

            builder.SetDeliveryTimestamp(DateTime.Now.AddSeconds(delay.TotalSeconds));

            return builder.Build();
        }
    }
}
