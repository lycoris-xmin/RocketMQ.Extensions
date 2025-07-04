namespace Lycoris.RocketMQ.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class RocketMqProducerOptions : RocketMqOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] Topics { get; internal set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public Type? Checker { get; internal set; }
    }
}
