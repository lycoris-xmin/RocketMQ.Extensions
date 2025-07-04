using Lycoris.RocketMQ.Extensions.Shared;
using Microsoft.Extensions.DependencyInjection;
using Org.Apache.Rocketmq;
using System.Collections.Concurrent;

namespace Lycoris.RocketMQ.Extensions.Impl
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class RocketProducerFactory : BaseProvider, IRocketProducerFactory
    {
        /// <summary>
        /// 生产者缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, Producer> _producers;
        private readonly IServiceProvider _serviceProvider;

        public RocketProducerFactory(IServiceProvider serviceProvider)
        {
            _producers = new ConcurrentDictionary<string, Producer>();
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Producer Create(string name)
        {
            var options = RocketOptionsStore.GetProducerOptions(name);

            if (options == null || string.IsNullOrEmpty(options.AccessKey) || string.IsNullOrEmpty(options.SecretKey) || string.IsNullOrEmpty(options.Endpoints))
                throw new InvalidOperationException($"{nameof(RocketMqProducerOptions)} named '{name}' is not configured");

            lock (_producers)
            {
                if (_producers.TryGetValue(name, out Producer? producer) && producer != null)
                    return producer;

                var config = this.GetClientConfig(options);

                var builder = new Producer.Builder().SetTopics(options.Topics).SetClientConfig(config);

                if (options.Checker != null)
                {
                    var checker = this._serviceProvider.GetRequiredKeyedService<ITransactionChecker>(name);
                    builder.SetTransactionChecker(checker);
                }

                producer = builder.Build().GetAwaiter().GetResult();

                _producers.AddOrUpdate(name, producer, (n, b) => producer);

                return producer;
            }
        }

        /// <summary>  
        ///  构建客户基础配置
        /// </summary>  
        /// <returns></returns>  
        private ClientConfig GetClientConfig(RocketMqProducerOptions options)
        {
            var credentialsProvider = new StaticSessionCredentialsProvider(options.AccessKey, options.SecretKey);
            var builder = new ClientConfig.Builder().SetEndpoints(options.Endpoints).SetCredentialsProvider(credentialsProvider);
            return builder.Build();
        }
    }
}
