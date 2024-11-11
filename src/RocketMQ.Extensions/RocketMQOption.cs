namespace RocketMQ.Extensions
{
    public class RocketMQOption
    {
        public string Endpoint { get; set; } = default!;

        public string AccessKey { get; set; } = default!;

        public string SecretKey { get; set; } = default!;

        public List<RocketMQTopicOption> TopicOptions { get; set; } = new List<RocketMQTopicOption>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class RocketMQTopicOption
    {
        public string Topic { get; set; } = default!;

        public string? Tag { get; set; }

        public string? Group { get; set; }
    }
}
