using SafetySharp.Modeling;
using SSharpÜbergang.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSharpÜbergang.Crossing 
{
    class CrossingTimer : Component
    {
        const int TIMER_LOWER_BOUND = 0;

        const int TIMER_UPPER_BOUND = 20;

        public virtual State CurrentState { get; private set; }

        [Range(TIMER_LOWER_BOUND, TIMER_UPPER_BOUND, OverflowBehavior.Clamp)]
        public int Countdown { get; private set; }

        public extern CrossingController.State controllerState { get; }

        private StateMachine<State> _myState = State.Idle;

        public CrossingTimer()
        {
            Countdown = TIMER_UPPER_BOUND;
        }


        public override void Update()
        {
            _myState
                .Transition
                    (
                    from: State.Idle,
                    to: State.Countdown,
                    guard: controllerState == CrossingController.State.Secured
                    )
                .Transition
                    (
                    from: State.Countdown,
                    to: State.Countdown,
                    guard: controllerState == CrossingController.State.Secured && Countdown != 0,
                    action: () => Countdown--
                    )
                .Transition
                    (
                    from: State.Countdown,
                    to: State.Idle,
                    guard: controllerState == CrossingController.State.Open,
                    action: () => Countdown = TIMER_UPPER_BOUND
                    )
                 .Transition
                    (
                    from: State.Countdown,
                    to: State.Triggered,
                    guard: Countdown == 0
                    );
        }

        public enum State
        {
            Idle,
            Countdown,
            Triggered
        }

        public readonly Fault FailingTimer = new PermanentFault();

        [FaultEffect(Fault = nameof(FailingTimer))]
        public class FailingTimerEffect : CrossingTimer
        {
            public override State CurrentState => State.Triggered;
        }
    }
}
