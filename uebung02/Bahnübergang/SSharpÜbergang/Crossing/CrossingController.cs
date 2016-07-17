using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSharpÜbergang.Crossing;
using SafetySharp.Modeling;
using SSharpÜbergang.Shared;

namespace SSharpÜbergang.Crossing
{
    class CrossingController : Component
    {
        [Hidden, Subcomponent]
        public CrossingSensor CrossingSensor;

        [Hidden, Subcomponent]
        public TrainLeftSensor TrainLeftSensor;

        [Hidden, Subcomponent]
        public CrossingTimer CrossingTimer;

        [Hidden, Subcomponent]
        public RadioModule RadioModule;

        [Hidden, Subcomponent]
        public CrossingMotor Motor;

        private StateMachine<State> _myState = State.WaitForMessage;

        public State CurrentState => _myState.State;

        public bool isSecured => _myState == State.Secured;

        public override void Update()
        {
            Update(CrossingSensor, TrainLeftSensor, CrossingTimer, RadioModule, Motor);
            _myState
                .Transition
                    (
                    from: State.WaitForMessage,
                    to: State.WaitForMessage,
                    guard: RadioModule.Receive() != Message.PleaseSecure
                    )
                .Transition
                    (
                    from: State.WaitForMessage,
                    to: State.Securing,
                    guard: RadioModule.Receive() == Message.PleaseSecure
                    )
                .Transition
                    (
                    from: State.Securing,
                    to: State.Secured,
                    guard: CrossingSensor.CurrentState == CrossingSensor.State.BarrierClosed
                    )
                .Transition
                    (
                    from: State.Secured,
                    to: State.Secured,
                    guard: RadioModule.Receive() == Message.PleaseConfirm,
                    action: () => RadioModule.Send(Message.IsSecured)
                    )
                .Transition
                    (
                    from: State.Secured,
                    to: State.Open,
                    guard: TrainLeftSensor.TrainLeftSignal || CrossingTimer.CurrentState == CrossingTimer.State.Triggered
                    )
                ;
        }


        public enum State
        {
            WaitForMessage,
            Securing,
            Secured,
            Open
        }
    }
}
