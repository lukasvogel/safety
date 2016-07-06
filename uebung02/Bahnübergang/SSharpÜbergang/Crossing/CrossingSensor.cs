using SafetySharp.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSharpÜbergang.Crossing
{
    class CrossingSensor : Component
    {
        public State CurrentState { get; private set; }

        public extern int Angle { get; }

        public CrossingSensor()
        {
            CurrentState = State.BarrierOpen;
        }

        public override void Update()
        {
            switch (Angle)
            {
                case 90:
                    CurrentState = State.BarrierOpen;
                    break;
                case 0:
                    CurrentState = State.BarrierClosed;
                    break;
                default:
                    CurrentState = State.NoContact;
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
