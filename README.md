# Remote WinVolume

This is a tool to change your volume and send media keys to your Windows device from any other device on your local network.

----
## Screenshots

![Error](https://i.imgur.com/bRBfHK4.png)
![Standard](https://i.imgur.com/CP1vPvm.png)

----
## Technology

The core program is written in C# and Python 3 is used to serve the web client.

Controlling the Windows volume is done with [AudioSwitcher's CoreAudioApi](https://github.com/xenolightning/AudioSwitcher/tree/master/AudioSwitcher.AudioApi.CoreAudio).

Media keys are handled with [InputSimulator](https://github.com/michaelnoonan/inputsimulator).

The WebSockets implementation has been made using [Fleck](https://github.com/statianzo/Fleck).

----
## Usage

For debugging, set the working directory to the AudioVolumeApp folder in Visual Studio.
When building a release, copy the AudioUI folder to the release folder.

To just use it, make sure to have python3 installed (bound to the py command), extract the contents of the zip to a folder and then just run `AudioVolumeApp.exe`. You can then navigate to `[YourLocalIP]:8000` on any device in your local network for the remote control.

----
## License

MIT, see LICENSE for details.
