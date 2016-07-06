using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;

namespace SSharpÜbergang.Shared
{
    class CommChannel : Component
    {
        public Message _message = Message.Nil;

        public virtual Message Receive()
        {
            Message result = _message;
            _message = Message.Nil;
            return result;
        }

        public virtual void Send(Message m)
        {
            _message = m;
        }

        public readonly Fault ChannelFault = new TransientFault();

        [FaultEffect(Fault = nameof(ChannelFault))]
        public class ChannelFaultEffect : CommChannel
        {
            public override void Send(Message m)
            {
                _message = Message.Nil;
            }
        }
    }
}
