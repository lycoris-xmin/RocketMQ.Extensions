using Microsoft.Extensions.DependencyInjection;
using RocketMQ.Extensions.Impl;

namespace RocketMQ.Extensions.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection AddProducerTransient<TService, TImplementation>(this IServiceCollection services) where TService : class, IRocketMQProducer where TImplementation : class, TService
        {
            services.AddTransient<TImplementation>()
                    .AddTransient<IProducerContext<TService>>(provider => new ProducerContext<TService>(provider.GetService<TImplementation>()));

            return services;
        }
    }
}
