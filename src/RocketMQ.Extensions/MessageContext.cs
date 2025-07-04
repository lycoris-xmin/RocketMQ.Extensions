using Org.Apache.Rocketmq;
using System.Text;

namespace Lycoris.RocketMQ.Extensions
{
    public class MessageContext
    {
        public MessageView Context { get; }

        public MessageContext(MessageView context)
        {
            Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public string MessageId => this.Context.MessageId;

        /// <summary>
        /// 
        /// </summary>
        public string Group => this.Context.MessageGroup;

        /// <summary>
        /// 
        /// </summary>
        public string Tag => this.Context.Tag;

        /// <summary>
        /// 
        /// </summary>
        public string Body => Encoding.UTF8.GetString(this.Context.Body);
    }
}
