using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;
using SSharpÜbergang.Environment;
using SSharpÜbergang.Shared;
using SSharpÜbergang.Train;

namespace SSharpÜbergang
{
    class Model : ModelBase
    {

        public Model()
        {

            var train = new Train.Train();
            var tc = new TrainController()
            {
                Odometer = new Odometer(),
                RadioModule = new RadioModule(),
                Brakes = new EmergencyBrakes()
            };

            var commChannel = new BidirectionalCommChannel();


            // Bind all of the ports of the odometer
            Bind(nameof(tc.Odometer.Position), nameof(train.Position));
            Bind(nameof(tc.Odometer.Speed), nameof(train.Speed));

            // Bind the communication channel
            Bind(nameof(tc.RadioModule.Channel), nameof(commChannel.DownChannel));
        }
    }
}
