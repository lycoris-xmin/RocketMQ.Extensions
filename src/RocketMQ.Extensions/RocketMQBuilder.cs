using Microsoft.Extensions.DependencyInjection;
using RocketMQ.Extensions.Extensions;

namespace RocketMQ.Extensions
{
    public class RocketMQBuilder
    {
        private readonly IServiceCollection services;

        public RocketMQBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public RocketMQBuilder Configure(Action<RocketMQOption> configure)
        {
            var option = new RocketMQOption();

            configure.Invoke(option);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        public RocketMQBuilder AddProducerService<TService, TImplementation>() where TService : class, IRocketMQProducer where TImplementation : class, TService
        {
            services.AddProducerTransient<TService, TImplementation>();
            return this;
        }
    }
}
