using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;

namespace SSharpÜbergang.Environment
{
    class CommChannel : Component
    {
        private Message _message;

        public Message Receive()
        {
            Message result = _message;
            _message = Message.Nil;
            return result;
        }

        public void Send(Message m)
        {
            _message = m;
        }
    }
}
