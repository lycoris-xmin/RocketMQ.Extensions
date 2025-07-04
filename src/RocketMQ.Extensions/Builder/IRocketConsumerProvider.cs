namespace Lycoris.RocketMQ.Extensions.Builder
{
    public interface IRocketConsumerProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ListenAsync();
    }
}
