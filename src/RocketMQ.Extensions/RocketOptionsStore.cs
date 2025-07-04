namespace Lycoris.RocketMQ.Extensions
{
    internal class RocketOptionsStore
    {
        private static readonly Dictionary<string, RocketMqProducerOptions> ProducerMap = new Dictionary<string, RocketMqProducerOptions>();

        private static readonly Dictionary<string, RocketMqProducerOptions> ConsumerMap = new Dictionary<string, RocketMqProducerOptions>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="options"></param>
        public static void AddOrUpdateProducerOptions(string key, RocketMqProducerOptions options)
        {
            if (ProducerMap.ContainsKey(key))
                ProducerMap[key] = options;
            else
                ProducerMap.Add(key, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RocketMqProducerOptions? GetProducerOptions(string key) => ProducerMap.ContainsKey(key) ? ProducerMap[key] : null;
    }
}
