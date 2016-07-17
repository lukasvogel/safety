using SafetySharp.Modeling;
using static SSharpÜbergang.Shared.Globals;

namespace SSharpÜbergang.Train
{
    class EmergencyBrakes : Component
    {
        public EmergencyBrakes()
        {
            Acceleration = 0;
        }


        [Range(0-MAX_DECEL, 0, OverflowBehavior.Clamp)]
        public virtual int Acceleration { get; private set; } = 0;

        public extern bool ShouldBrake { get; }

        public readonly Fault FailingBrakes = new PermanentFault();

        public override void Update()
        {
            if (ShouldBrake)
                Acceleration -= 1;
        }

        [FaultEffect(Fault = nameof(FailingBrakes))]
        public class FailingBrakesEffect : EmergencyBrakes
        {
            public override int Acceleration => 0;
        }

    }
}
