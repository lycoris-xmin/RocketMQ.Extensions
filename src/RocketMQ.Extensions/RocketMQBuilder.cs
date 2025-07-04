using Lycoris.RocketMQ.Extensions.Builder.Impl;
using Lycoris.RocketMQ.Extensions.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions
{
    public class RocketMqBuilder : RocketMqOptions
    {
        internal Dictionary<string, RocketMqProducerOptions> ProducerOptions = new Dictionary<string, RocketMqProducerOptions>();

        private readonly IServiceCollection _services;

        public RocketMqBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topics"></param>
        /// <returns></returns>
        public RocketMqBuilder AddProducer<T>(params string[] topics) where T : BaseRocketProducerService
        {
            var producer = typeof(T);
            if (producer.IsAbstract)
                throw new Exception("producer service does not support abstract class registration");

            var mapKey = producer.FullName!;

            this.ProducerOptions.Add(mapKey, new RocketMqProducerOptions() { Topics = topics });

            _services.TryAddKeyedTransient<IRocketProducerService, T>(mapKey);

            _services.TryAddSingleton<IRocketProducerFactory, RocketProducerFactory>();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topics"></param>
        /// <returns></returns>
        public RocketMqBuilder AddProducer<T, TChecker>(params string[] topics) where T : BaseRocketProducerService where TChecker : class, ITransactionChecker
        {
            var producer = typeof(T);
            if (producer.IsAbstract)
                throw new Exception("producer service does not support abstract class registration");

            var checker = typeof(TChecker);
            if (checker.IsAbstract)
                throw new Exception("transaction checker does not support abstract class registration");

            var mapKey = producer.FullName!;

            this.ProducerOptions.Add(mapKey, new RocketMqProducerOptions() { Topics = topics, Checker = checker });

            _services.TryAddKeyedTransient<IRocketProducerService, T>(mapKey);

            _services.TryAddKeyedTransient<ITransactionChecker, TChecker>(mapKey);

            _services.TryAddSingleton<IRocketProducerFactory, RocketProducerFactory>();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configure"></param>
        /// <returns></returns>
        public RocketMqBuilder AddSimpleConsumer<T>(Action<RocketMqConsumerOptions> configure) where T : class, IRocketConsumer
        {
            var consumer = typeof(T);
            if (consumer.IsAbstract)
                throw new Exception("consumer service does not support abstract class registration");

            var mapKey = consumer.FullName!;

            var options = new RocketMqConsumerOptions();

            configure.Invoke(options);

            var builder = new DefaultRocketConsumerBuilder(this._services, options);

            builder.AddSimpleConsumer<T>();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configure"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public RocketMqBuilder AddPushConsumer<T>(Action<RocketMqConsumerOptions> configure) where T : class, IRocketConsumer
        {
            var consumer = typeof(T);
            if (consumer.IsAbstract)
                throw new Exception("consumer service does not support abstract class registration");

            var mapKey = consumer.FullName!;

            var options = new RocketMqConsumerOptions();

            configure.Invoke(options);

            var builder = new DefaultRocketConsumerBuilder(this._services, options);

            builder.AddPushConsumer<T>();

            return this;
        }
    }
}
