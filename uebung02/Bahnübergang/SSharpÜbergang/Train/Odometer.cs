using SafetySharp.Modeling;

namespace SSharpÜbergang.Train
{
    class Odometer : Component
    {

        // CONSTS
        private readonly int FAULT_SPEED = 1;
        private readonly int FAULT_POS_OFFSET = 20;

        // INPUTS
        public extern int Position { get; }
        public extern int Speed { get; }


        // OUTPUTS
        public virtual int ReportedPosition => Position;
        public virtual int ReportedSpeed => Speed;


        // no update needed as an odometer has no inherent states

        // FAULTS
        public readonly Fault OdometerPos = new PermanentFault();
        public readonly Fault OdometerSpeed = new TransientFault();


        // FAULT EFFECTS
        [FaultEffect(Fault = nameof(OdometerPos))]
        public class OdometerPosEffect : Odometer
        {
            public override int ReportedPosition => base.Position - FAULT_POS_OFFSET;
        }

        [FaultEffect(Fault = nameof(OdometerSpeed))]
        public class OdometerSpeedEffect : Odometer
        {
            public override int ReportedSpeed => FAULT_SPEED;
        }

    }
}
