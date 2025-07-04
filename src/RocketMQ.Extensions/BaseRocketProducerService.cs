using Microsoft.Extensions.DependencyInjection;
using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions
{
    public class BaseRocketProducerService : IRocketProducerService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IRocketProducerFactory _factory;

        /// <summary>
        /// 
        /// </summary>
        protected string ProducerMapKey { get => GetType().FullName!; }

        /// <summary>
        /// 
        /// </summary>
        protected RocketMqProducerOptions Options { get => Producer.Options; }

        private BaseProducer? _producer;
        /// <summary>
        /// 
        /// </summary>
        protected BaseProducer Producer
        {
            get
            {
                var producer = _factory.Create(ProducerMapKey);
                var options = RocketOptionsStore.GetProducerOptions(ProducerMapKey) ?? new RocketMqProducerOptions();

                _producer ??= new BaseProducer(producer, options);

                return _producer;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public BaseRocketProducerService(IServiceProvider provider) => _factory = provider.GetRequiredService<IRocketProducerFactory>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Producer GetProducer() => Producer.Producer;
    }
}
