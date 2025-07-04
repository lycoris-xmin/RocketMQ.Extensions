using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions.Shared
{
    internal class BaseProvider
    {
        /// <summary>  
        ///  构建客户基础配置
        /// </summary>  
        /// <returns></returns>  
        protected ClientConfig GetClientConfig(RocketMqOptions options)
        {
            var credentialsProvider = new StaticSessionCredentialsProvider(options.AccessKey, options.SecretKey);
            var builder = new ClientConfig.Builder().SetEndpoints(options.Endpoints).SetCredentialsProvider(credentialsProvider);
            return builder.Build();
        }
    }
}
