using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions.Builder.Impl
{
    public class DefaultRocketConsumerBuilder : IRocketConsumerBulder
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection Services { get; }

        public RocketMqConsumerOptions Options { get; }

        public DefaultRocketConsumerBuilder(IServiceCollection services, RocketMqConsumerOptions options)
        {
            Services = services;
            Options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRocketConsumerBulder AddSimpleConsumer<T>() where T : class, IRocketConsumer
        {
            var key = typeof(T).FullName;

            Services.TryAddKeyedTransient<IRocketConsumer, T>(key);

            Services.AddSingleton<IRocketConsumerProvider>(sp =>
            {
                return new DefaultRocketConsumerProvider(sp, Options, async (result) =>
                {
                    using var scope = sp.CreateScope();
                    var consumer = scope.ServiceProvider.GetRequiredKeyedService<T>(key);

                    try
                    {
                        await consumer.InvokeAsync(result);
                        return ConsumeResult.SUCCESS;
                    }
                    catch (Exception ex)
                    {
                        await consumer.ExceptionHandlerAsync(result, ex);
                        return ConsumeResult.FAILURE;
                    }
                });
            });

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRocketConsumerBulder AddPushConsumer<T>() where T : class, IRocketConsumer
        {
            var key = typeof(T).FullName;

            Services.TryAddKeyedTransient<IRocketConsumer, T>(key);

            Services.AddSingleton<IRocketConsumerProvider>(sp =>
            {
                return new DefaultRocketConsumerProvider(sp, Options, () =>
                {
                    using var scope = sp.CreateScope();
                    var consumer = scope.ServiceProvider.GetRequiredKeyedService<T>(key);
                    return new DefaultMessageListener(consumer.InvokeAsync, consumer.ExceptionHandlerAsync);
                });
            });

            return this;
        }
    }
}
