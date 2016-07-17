using SafetySharp.Modeling;
using SSharpÜbergang.Shared;
using static SSharpÜbergang.Shared.Globals;

namespace SSharpÜbergang.Train
{
    class TrainController : Component
    {
        [Hidden, Subcomponent] public EmergencyBrakes Brakes;
        [Hidden, Subcomponent] public Odometer Odometer;
        [Hidden, Subcomponent] public RadioModule RadioModule;

        private  StateMachine<State> _myState = State.Idle;

        public State CurrentState =>  _myState.State;

        public bool ShouldBrake => _myState.State == State.NoConfirmReceived;


        private int BEP => GP - 2 - (Odometer.ReportedSpeed* Odometer.ReportedSpeed);
        private int AP => BEP - 2*Odometer.ReportedSpeed;
        private int EP => AP - Odometer.ReportedSpeed*(1 + Odometer.ReportedSpeed);
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
                    guard: Odometer.ReportedPosition < BEP && RadioModule.Receive() == Message.IsSecured,
                    action: () => RadioModule.Send(Message.Nil)
                )
                .Transition(
                    from: State.WaitForConfirm,
                    to: State.NoConfirmReceived,
                    guard: Odometer.ReportedPosition >= BEP,
                    action: () => RadioModule.Send(Message.Nil)

                );


        }

        public enum State
        {
            Idle,
            WaitForConfirm,
            NoConfirmReceived,
            ConfirmReceived
        }
    }
}
