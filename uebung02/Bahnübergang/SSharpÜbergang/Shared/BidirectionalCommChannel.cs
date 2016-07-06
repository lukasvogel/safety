using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;

namespace SSharpÜbergang.Shared
{
    class BidirectionalCommChannel : Component
    {
        public CommChannel UpChannel { get; } = new CommChannel();
        public CommChannel DownChannel { get; } = new CommChannel();
    }
}
