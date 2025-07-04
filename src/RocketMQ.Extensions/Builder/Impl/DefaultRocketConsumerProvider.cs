using Lycoris.RocketMQ.Extensions.Shared;
using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions.Builder.Impl
{
    internal class DefaultRocketConsumerProvider : BaseProvider, IRocketConsumerProvider
    {
        private readonly IServiceProvider _provider;
        private readonly RocketMqConsumerOptions _options;
        private readonly Func<MessageContext, Task<ConsumeResult>>? _simpleListener;
        private readonly IMessageListener? _pushListener;

        public DefaultRocketConsumerProvider(IServiceProvider provider, RocketMqConsumerOptions options, Func<MessageContext, Task<ConsumeResult>> listener)
        {
            _provider = provider;
            _options = options;
            _simpleListener = listener;
        }

        public DefaultRocketConsumerProvider(IServiceProvider provider, RocketMqConsumerOptions options, Func<IMessageListener> listener)
        {
            _provider = provider;
            _options = options;
            _pushListener = listener.Invoke();
        }

        public async Task ListenAsync()
        {
            if (_simpleListener != null)
                await SimpleListenAsync();
            else
                await PushListenAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task SimpleListenAsync()
        {
            var config = GetClientConfig(_options);

            var builder = new SimpleConsumer.Builder();

            builder.SetClientConfig(config);

            builder.SetConsumerGroup(_options.Group);

            builder.SetAwaitDuration(_options.TimeSpan);

            builder.SetSubscriptionExpression(_options.Subscription);

            var consumer = await builder.Build();

            while (true)
            {
                var messageViews = await consumer.Receive(_options.Count, _options.TimeSpan);
                if (messageViews != null && messageViews.Count > 0)
                {
                    foreach (var item in messageViews)
                    {
                        var result = await _simpleListener!.Invoke(new MessageContext(item));

                        if (result == ConsumeResult.SUCCESS)
                            await consumer.Ack(item);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task PushListenAsync()
        {
            var config = GetClientConfig(_options);

            var builder = new PushConsumer.Builder();

            builder.SetClientConfig(config);

            builder.SetConsumerGroup(_options.Group);

            builder.SetSubscriptionExpression(_options.Subscription);

            builder.SetMessageListener(_pushListener);

            await builder.Build();
        }
    }
}
