using SafetySharp.Modeling;
using SSharpÜbergang.Environment;
using SSharpÜbergang.Shared;

namespace SSharpÜbergang.Train
{
    class TrainController : Component
    {
        [Hidden, Subcomponent] public EmergencyBrakes Brakes;
        [Hidden, Subcomponent] public Odometer Odometer;
        [Hidden, Subcomponent] public RadioModule RadioModule;

        private readonly StateMachine<State> _myState = State.Idle;

        private int EP = 10;
        private int AP = 30;
        private int BEP = 80;
        private int GP = 95;
        private int SP = 100;

        public override void Update()
        {
            Update(Odometer,Brakes,RadioModule);

            _myState
                .Transition(
                    from: State.Idle,
                    to: State.Idle,
                    guard: Odometer.ReportedPosition >= EP && Odometer.ReportedPosition < AP,
                    action: () => RadioModule.Send(Message.PleaseSecure)
                )
                .Transition(
                    from: State.Idle,
                    to: State.WaitForConfirm,
                    guard: Odometer.ReportedPosition >= AP,
                    action: () => RadioModule.Send(Message.PleaseConfirm)
                )
                .Transition(
                    from: State.WaitForConfirm,
                    to: State.ConfirmReceived,
                    guard: Odometer.ReportedPosition < BEP && RadioModule.Receive() == Message.IsSecured
                )
                .Transition(
                    from: State.WaitForConfirm,
                    to: State.NoConfirmReceived,
                    guard: Odometer.ReportedPosition >= BEP && RadioModule.Receive() != Message.IsSecured
                );


        }

        private enum State
        {
            Idle,
            WaitForConfirm,
            NoConfirmReceived,
            ConfirmReceived
        }
    }
}
