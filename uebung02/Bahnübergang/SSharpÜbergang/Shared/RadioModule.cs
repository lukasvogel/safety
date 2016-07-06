using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;
using SSharpÜbergang.Shared;

namespace SSharpÜbergang.Shared
{
    class RadioModule : Component
    {

        public extern CommChannel SendChannel { get; }
        public extern CommChannel RecvChannel { get; }

        public Message Receive() => RecvChannel.Receive();
        public void Send(Message m) => SendChannel.Send(m);
    }
}
