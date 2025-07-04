using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions
{
    public interface IRocketConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        Task<ConsumeResult> InvokeAsync(MessageContext messages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        Task ExceptionHandlerAsync(MessageContext messages, Exception ex);
    }
}
