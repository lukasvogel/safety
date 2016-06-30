using SafetySharp.Modeling;

namespace SSharpÜbergang.Train
{
    class EmergencyBrakes : Component
    {


        const int MAX_BRAKE_DECEL = -1;

        [Range(MAX_BRAKE_DECEL,0, OverflowBehavior.Clamp)]
        public virtual int Acceleration { get; private set; }


        public readonly Fault FailingBrakes = new PermanentFault();

        [FaultEffect(Fault = nameof(FailingBrakes))]
        public class FailingBrakesEffect : EmergencyBrakes
        {
            public override int Acceleration => 0;
        }

    }
}
