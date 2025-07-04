using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions.Builder.Impl
{
    internal class DefaultMessageListener : IMessageListener
    {
        private readonly Func<MessageContext, Task<ConsumeResult>> _listener;
        private readonly Func<MessageContext, Exception, Task> _exceptionListener;

        public DefaultMessageListener(Func<MessageContext, Task<ConsumeResult>> listener, Func<MessageContext, Exception, Task> exceptionListener)
        {
            _listener = listener;
            _exceptionListener = exceptionListener;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageView"></param>
        /// <returns></returns>
        public ConsumeResult Consume(MessageView messageView)
        {
            var body = new MessageContext(messageView);

            try
            {
                return _listener.Invoke(body).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _exceptionListener.Invoke(body, ex).GetAwaiter().GetResult();
                return ConsumeResult.FAILURE;
            }
        }
    }
}
