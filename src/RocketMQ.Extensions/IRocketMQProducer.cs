using Org.Apache.Rocketmq;

namespace RocketMQ.Extensions
{
    public interface IRocketMQProducer
    {
        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishAsync(string topic);

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishAsync(string topic, string body);

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <param name="tag">标志(路由)</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishAsync(string topic, string tag, string body);

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="messageId">消息Id</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishAsync(string topic, string tag, string body, string messageId);

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishFifoAsync(string group, string topic);

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishFifoAsync(string group, string topic, string body);

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishFifoAsync(string group, string topic, string tag, string body);

        /// <summary>
        /// 发送顺序消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishFifoAsync(string group, string topic, string tag, string body, string messageId);

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishDelayAsync(string topic, TimeSpan delay);

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="body">消息内容</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishDelayAsync(string topic, string body, TimeSpan delay);

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishDelayAsync(string topic, string tag, string body, TimeSpan delay);

        /// <summary>
        /// 发送延迟消息
        /// </summary>
        /// <param name="topic">主题(队列)</param>
        /// <param name="tag">标志(路由)</param>
        /// <param name="body">消息内容</param>
        /// <param name="messageId">消息Id</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        Task<ISendReceipt?> PublishDelayAsync(string topic, string tag, string body, string messageId, TimeSpan delay);
    }
}
