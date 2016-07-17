using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SafetySharp.Modeling;
using SSharpÜbergang.Shared;
using SSharpÜbergang.Train;
using SSharpÜbergang.Crossing;
using SafetySharp.Analysis;

namespace SSharpÜbergang
{
    class Model : ModelBase
    {

        [Root(RootKind.Controller)]
        TrainController tc;

        [Root(RootKind.Controller)]
        CrossingController cc;

        [Root(RootKind.Plant)]
        Train.Train train;

        [Root(RootKind.Plant)]
        BidirectionalCommChannel commChannel;

        [Root(RootKind.Plant)]
        Crossing.Crossing Crossing;

        public Model()
        {

            train = new Train.Train();
            
            tc = new TrainController()
            {
                Odometer = new Odometer(),
                RadioModule = new RadioModule(),
                Brakes = new EmergencyBrakes()
            };

            commChannel = new BidirectionalCommChannel();

            // Bind all of the ports of the odometer
            Bind(nameof(tc.Odometer.Position), nameof(train.Position));
            Bind(nameof(tc.Odometer.Speed), nameof(train.Speed));

            // Bind the emergency brakes to the train
            Bind(nameof(train.Acceleration), nameof(tc.Brakes.Acceleration));

            // Bind controller to breaks
            Bind(nameof(tc.Brakes.ShouldBrake), nameof(tc.ShouldBrake));


            Crossing = new Crossing.Crossing();

            cc = new CrossingController
            {
                CrossingSensor = new CrossingSensor(),
                CrossingTimer = new CrossingTimer(),
                Motor = new CrossingMotor(),
                RadioModule = new RadioModule(),
                TrainLeftSensor = new TrainLeftSensor()
            };

            Bind(nameof(cc.CrossingSensor.Angle), nameof(cc.Motor.Angle));
            Bind(nameof(cc.Motor.ControllerState), nameof(cc.CurrentState));
            Bind(nameof(cc.Motor.CrossingSesnsorState), nameof(cc.CrossingSensor.CurrentState));
            Bind(nameof(cc.CrossingTimer.controllerState), nameof(cc.CurrentState));


            Bind(nameof(cc.TrainLeftSensor.Position), nameof(train.Position));


            // Bind the communication channels
            Bind(nameof(tc.RadioModule.SendChannel), nameof(commChannel.DownChannel));
            Bind(nameof(tc.RadioModule.RecvChannel), nameof(commChannel.UpChannel));

            Bind(nameof(cc.RadioModule.SendChannel), nameof(commChannel.UpChannel));
            Bind(nameof(cc.RadioModule.RecvChannel), nameof(commChannel.DownChannel));

        }

        public Formula Hazard => train.Position >= Globals.GP && cc.Motor.Angle != 0 && train.Position <= Globals.SP;
        
        public static void Main()
        {

            Console.WriteLine("Hallo");

            var m = new Model();

            //PrintCounterExample(results.CounterExample);


            //DCCA
            var safetyRestul = SafetyAnalysis.AnalyzeHazard(m, m.Hazard);
            Console.WriteLine(safetyRestul);

            foreach (var x in safetyRestul.CounterExamples)
            {
                Console.WriteLine($"Counter ex {x.Key}:");
                PrintCounterExample(x.Value);
            }

            Console.ReadLine();
        }

        private static void PrintCounterExample(CounterExample counterExample)
        {
            var s = new Simulator(counterExample);

            var m = (Model)s.Model;

            var i = 0;

            do
            {
                Console.WriteLine($"Step {i} ====================");

                Console.WriteLine($"Train Pos/Reported: {m.train.Position}/{m.tc.Odometer.ReportedPosition}");
                Console.WriteLine($"Train Speed/Reported: {m.train.Speed}/{m.tc.Odometer.ReportedSpeed}");
                Console.WriteLine($"Train Accel: {m.train.Acceleration}");
                Console.WriteLine($"Up Channel: {m.commChannel.UpChannel._message}");
                Console.WriteLine($"Down Channel: {m.commChannel.DownChannel._message}");
                Console.WriteLine($"tc state: {m.tc.CurrentState}");
                Console.WriteLine($"cc state: {m.cc.CurrentState}");

                Console.WriteLine($"Motor s({m.cc.Motor.CurrentState}){m.cc.Motor.Angle}");

                foreach (var f in m.Faults)
                {
                    if (f.IsActivated)
                    {
                        Console.WriteLine($"Fault {f.Name} : {f.IsActivated}");

                    }
                }


                ++i;
            } while (s.SimulateStep());
        }

    }
}
