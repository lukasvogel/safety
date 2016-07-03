using SafetySharp.Modeling;

namespace SSharpÜbergang.Train
{
    class EmergencyBrakes : Component
    {
        public EmergencyBrakes()
        {
            Acceleration = 0;
        }

        const int MAX_BRAKE_DECEL = -5;

        [Range(MAX_BRAKE_DECEL,0, OverflowBehavior.Clamp)]
        public virtual int Acceleration { get; private set; }

        public extern bool ShouldBreak { get; }

        public readonly Fault FailingBrakes = new PermanentFault();

        public override void Update()
        {
            if (ShouldBreak)
                Acceleration -= 1;
        }

        [FaultEffect(Fault = nameof(FailingBrakes))]
        public class FailingBrakesEffect : EmergencyBrakes
        {
            public override int Acceleration => 0;
        }

    }
}
