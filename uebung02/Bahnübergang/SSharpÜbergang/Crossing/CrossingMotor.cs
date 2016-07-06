using SafetySharp.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSharpÜbergang.Crossing
{
    class CrossingMotor : Component
    {
        const int BARRIER_SPEED = 45;
        const int MAX_ANGLE = 90;

        public State CurrentState => _myState.State;

        private StateMachine<State> _myState = State.Off;

        public extern CrossingController.State ControllerState { get; }

        public extern CrossingSensor.State CrossingSesnsorState { get; }

        [Range(0, MAX_ANGLE, OverflowBehavior.Clamp)]
        public int Angle { get; private set; }

        public CrossingMotor()
        {
            Angle = MAX_ANGLE;
        }

        public override void Update()
        {
            _myState
                .Transition
                    (
                    from: State.Off,
                    to: State.Open,
                    guard: ControllerState == CrossingController.State.Open && CrossingSesnsorState != CrossingSensor.State.BarrierOpen
                    )
                .Transition
                    (
                    from: State.Open,
                    to: State.Open,
                    guard: CrossingSesnsorState != CrossingSensor.State.BarrierOpen,
                    action: () => Angle += BARRIER_SPEED
                    )
                .Transition
                    (
                    from: State.Open,
                    to: State.Off,
                    guard: CrossingSesnsorState == CrossingSensor.State.BarrierOpen
                    )
                .Transition
                    (
                    from: State.Off,
                    to: State.Close,
                    guard: ControllerState == CrossingController.State.Securing && CrossingSesnsorState != CrossingSensor.State.BarrierClosed
                    )
                .Transition
                    (
                    from: State.Close,
                    to: State.Close,
                    guard: CrossingSesnsorState != CrossingSensor.State.BarrierClosed,
                    action: () => Angle -= BARRIER_SPEED
                    )
                .Transition
                    (
                    from: State.Close,
                    to: State.Off,
                    guard: CrossingSesnsorState == CrossingSensor.State.BarrierClosed
                    );
        }

        public readonly Fault FailingMotor= new PermanentFault();

        [FaultEffect(Fault = nameof(FailingMotor))]
        public class FailingTimerEffect : CrossingMotor
        {
            public override void Update()
            {
                // do nothing
            }
        }

        public enum State
        {
            Off,
            Close,
            Open
        }
    }
}
