using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;
using static SSharpÜbergang.Shared.Globals;

namespace SSharpÜbergang.Crossing
{
    class TrainLeftSensor : Component
    {
        public bool TrainLeftSignal { get; private set; }
        public extern int Position { get; }

        public readonly Fault SensorFault = new TransientFault();

        public override void Update()
        {
            if (!TrainLeftSignal && Position >= SP && Position <= SP + 10)
                TrainLeftSignal = true;
            
            if (TrainLeftSignal && Position < SP)
                TrainLeftSignal = true;
        }

        [FaultEffect(Fault = nameof(SensorFault))]
        public class SensorFaultEffect : TrainLeftSensor
        {
            public override void Update()
            {
                TrainLeftSignal = true;
            }
        }
    }
}
