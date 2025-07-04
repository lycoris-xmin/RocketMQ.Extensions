using Org.Apache.Rocketmq;

namespace Lycoris.RocketMQ.Extensions
{
    public class RocketMqConsumerOptions : RocketMqOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, FilterExpression> Subscription { get; internal set; } = new Dictionary<string, FilterExpression>();

        /// <summary>
        /// 拉模式下必须设置
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 拉模式下必须设置
        /// </summary>
        public TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(15);
    }
}
