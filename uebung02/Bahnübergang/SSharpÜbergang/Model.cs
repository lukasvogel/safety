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

            // Bind the communication channels
            Bind(nameof(tc.RadioModule.SendChannel), nameof(commChannel.DownChannel));
            Bind(nameof(tc.RadioModule.RecvChannel), nameof(commChannel.UpChannel));

            // Bind the emergency brakes to the train
            Bind(nameof(train.Acceleration), nameof(tc.Brakes.Acceleration));

            // Bind controller to breaks
            Bind(nameof(EmergencyBrakes.ShouldBreak), nameof(tc.ShouldBreak));

        }
    }
}
