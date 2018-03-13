# Remote WinVolume

Currently simply a functional proof of concept, this is a tool to change the volume of your Windows device from any other device on your local network.

----
## Technology

The core program is written in C# and Python 3 is used to serve the web client.

Controlling the Windows volume is done with [AudioSwitcher's CoreAudioApi](https://github.com/xenolightning/AudioSwitcher/tree/master/AudioSwitcher.AudioApi.CoreAudio).

The WebSockets implementation has been made using [Fleck](https://github.com/statianzo/Fleck).

----
## Usage

Once you've either cloned or downloaded this repository, run `AudioVolumeApp.exe`, located in `AudioVolumeApp/bin/Release/` and `run-server.bat`, located in `AudioVolumeApp/AudioUI/`. This assumes you run Python 3 with the `py` command, change it to `python3` if your installation of Python requires that. Once both servers run, you can visit the remote control page on `[YourLocalIP]:8080` on any device in your local network.

----
## License

MIT, see LICENSE for details.
