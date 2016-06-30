using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;
using SSharpÜbergang.Environment;

namespace SSharpÜbergang.Shared
{
    class RadioModule : Component
    {

        public extern CommChannel Channel { get; }

        public Message Receive() => Channel.Receive();
        public void Send(Message m) => Channel.Send(m);
    }
}
