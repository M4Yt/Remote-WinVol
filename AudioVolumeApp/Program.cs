using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Fleck;

namespace AudioVolumeApp
{
    class Program
    {
        public static CoreAudioDevice Master = new CoreAudioController().DefaultPlaybackDevice;
        public static List<IWebSocketConnection> AllSockets = new List<IWebSocketConnection>();
        public static WebSocketServer Server = new WebSocketServer("ws://0.0.0.0:8181");
        static void Main(string[] args)
        {
            var observer = new VolumeObserver();
            Master.VolumeChanged.Subscribe(observer);

            Server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    AllSockets.Add(socket);
                    socket.Send(GetVolume().ToString());
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    AllSockets.Remove(socket);
                };
                //socket.OnMessage = message => socket.Send(message);
                socket.OnMessage = message => SetVolume(Double.Parse(message));
            });

            while (true)
            {
                double input = Double.Parse(Console.ReadLine());
                Master.Volume = input;
            }
        }

        public static double GetVolume()
        {
            return Master.Volume;
        }

        public static void SetVolume(double volume)
        {
            try
            {
                Master.Volume = volume;
            }
            catch
            {
                Console.WriteLine("Something went wrong?");
            }
        }
    }
    class VolumeObserver : IObserver<AudioSwitcher.AudioApi.DeviceVolumeChangedArgs>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DeviceVolumeChangedArgs value)
        {
            Console.WriteLine("Volume changed to: " + Program.GetVolume());
            foreach (var socket in Program.AllSockets.ToList())
            {
                socket.Send(Program.GetVolume().ToString());
            }
        }
    }
}
