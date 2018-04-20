using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Fleck;
using WindowsInput;

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
            var mkh = new MediaKeyHandler();
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
                socket.OnMessage = message =>
                {
                    if (Double.TryParse(message, out double newVol))
                    {
                        SetVolume(newVol);
                    } else
                    {
                        switch (message)
                        {
                            case "playpause":
                                {
                                    mkh.PlayPause();
                                    break;
                                }
                            case "stop":
                                {
                                    mkh.Stop();
                                    break;
                                }
                            case "prev":
                                {
                                    mkh.PreviousTrack();
                                    break;
                                }
                            case "next":
                                {
                                    mkh.NextTrack();
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                };
            });

            Task.Run(() =>
            {
                var uiServer = new Process();

                uiServer.StartInfo.UseShellExecute = false;
                uiServer.StartInfo.FileName = "py";
                uiServer.StartInfo.Arguments = "-m http.server";
                uiServer.StartInfo.WorkingDirectory = "AudioUI";

                uiServer.Start();
                uiServer.WaitForExit();
            });
            
            while (true)
            {
                if (Double.TryParse(Console.ReadLine(), out double input))
                {
                    Master.Volume = input;
                }
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

    class MediaKeyHandler
    {
        private InputSimulator s = new InputSimulator();

        public void PlayPause()
        {
            s.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_PLAY_PAUSE);
            Console.WriteLine("PlayPause Pressed");
        }

        public void Stop()
        {
            s.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_STOP);
            Console.WriteLine("Stop Pressed");
        }

        public void PreviousTrack()
        {
            s.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_PREV_TRACK);
            Console.WriteLine("PrevTrack Pressed");
        }

        public void NextTrack()
        {
            s.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MEDIA_NEXT_TRACK);
            Console.WriteLine("NextTrack Pressed");
        }
    }
}
