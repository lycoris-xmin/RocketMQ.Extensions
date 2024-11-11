using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Org.Apache.Rocketmq;
using RocketMQ.Extensions.Impl;

namespace RocketMQ.Extensions
{
    public static class RocketMQExtensions
    {
        public static IServiceCollection AddRocketMQExtension(this IServiceCollection services, Action<RocketMQBuilder> configure)
        {
            var builder = new RocketMQBuilder(services);

            configure.Invoke(builder);

            services.AddRocketMQClientConfig();

            services.TryAddTransient<IRocketMQProducer, BaseRocketMQProducer>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static void AddRocketMQClientConfig(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var option = sp.GetRequiredService<IOptions<RocketMQOption>>().Value;

                var builder = new ClientConfig.Builder().SetEndpoints(option.Endpoint);

                if (!string.IsNullOrEmpty(option.AccessKey) && !string.IsNullOrEmpty(option.SecretKey))
                {
                    var credentialsProvider = new StaticSessionCredentialsProvider(option.AccessKey, option.SecretKey);
                    builder.SetCredentialsProvider(credentialsProvider);
                }

                return builder.Build();
            });
        }
    }
}
