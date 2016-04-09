using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mindstorm1
{
    class Program
    {
        private static Brick _brick;

        static void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            foreach (var port in e.Ports)
            {
                Console.WriteLine(port.Key + "  " + port.Value);
            }
        }

        static async void MainAsync()
        {
            var conType = new BluetoothCommunication("COM5");

            if (conType != null)
            {
                _brick = new Brick(conType, true);
                _brick.BrickChanged += _brick_BrickChanged;
                try
                {
                    await _brick.ConnectAsync();
                    var output = OutputPort.A;
                    await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(output, 50, 0, 2000, 0, false);
                    Thread.Sleep(3000);
                    _brick.Disconnect();

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    _brick.Disconnect();
                }
            }
        }

        private static Task DoSomething()
        {

            return Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(".");
                }
            });
        }
        static void Main(string[] args)
        {
            Console.WriteLine("To Exit enter q and press enter");

            while (true)
            {
                var enteredData = Console.ReadLine();

                if (enteredData.ToLower() == "f")
                {
                    AsyncContext.Run(() => MainAsync());
                }
                if (enteredData.ToLower() == "q")
                {
                    break;
                }
            }
        }
    }
}
