using SafetySharp.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSharpÜbergang.Crossing
{
    class BarrierSensor : Component
    {
        public State Barrier { get; private set; }

        public extern int Angle { get; }

        public override void Update()
        {

            switch (Angle)
            {
                case 90:
                    Barrier = State.BarrierOpen;
                    break;
                case 0:
                    Barrier = State.BarrierClosed;
                    break;
                default:
                    Barrier = State.NoContact;
                    break;
            }
        }

        public enum State
        {
            BarrierOpen,
            BarrierClosed,
            NoContact
        }
    }
}
