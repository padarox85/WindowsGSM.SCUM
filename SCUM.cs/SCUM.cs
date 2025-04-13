using System;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Engine;
using WindowsGSM.GameServer.Query;

namespace WindowsGSM.Plugins
{
    public class SCUM : SteamCMDAgent
    {
        public Plugin Plugin = new Plugin
        {
            name = "WindowsGSM.SCUM",
            author = "Padarox85",
            description = "🧩 WindowsGSM plugin for SCUM Dedicated Server",
            version = "1.0",
            url = "https://github.com/padarox85/WindowsGSM.SCUM",
            color = "#9eff99"
        };

        
        public SCUM(ServerConfig serverData) : base(serverData) => base.serverData = _serverData = serverData;
        private readonly ServerConfig _serverData;
        
        public override bool loginAnonymous => true;
        public override string AppId => ""; // Missing atm and updated when Dedicated Server is released
        
        public override string StartPath => ""; // Game server start path, missing atm and updated when Dedicated Server is released
        public string FullName = "SCUM Dedicated Server"; // Game server FullName
        public bool AllowsEmbedConsole = false;  // Missing atm and updated when Dedicated Server is released
        public int PortIncrements = 2; // This tells WindowsGSM how many ports should skip after installation, updated when Dedicated Server is released
        public object QueryMethod = null; // Query method should be use on current server type. Accepted value: null or new A2S() or new FIVEM() or new UT3()
        // Missing atm and updated when Dedicated Server is released


        // - Game server default values
        // Missing atm and updated when Dedicated Server is released


        // - Create a default cfg for the game server after installation
        public async void CreateServerCFG() { }


        // - Start server function, return its Process to WindowsGSM
        public async Task<Process> Start()
        {
            // Prepare start parameter
            var param = new StringBuilder();
            param.Append(string.IsNullOrWhiteSpace(_serverData.ServerPort) ? string.Empty : $" -port={_serverData.ServerPort}");
            param.Append(string.IsNullOrWhiteSpace(_serverData.ServerName) ? string.Empty : $" -name=\"{_serverData.ServerName}\"");
            param.Append(string.IsNullOrWhiteSpace(_serverData.ServerParam) ? string.Empty : $" {_serverData.ServerParam}");
 
            // Prepare Process
            var p = new Process
            {
                StartInfo =
                {
                    WindowStyle = ProcessWindowStyle.Minimized,
                    UseShellExecute = false,
                    WorkingDirectory = ServerPath.GetServersServerFiles(_serverData.ServerID),
                    FileName = ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath),
                    Arguments = param.ToString()
                },
                EnableRaisingEvents = true
            };

            // Start Process
            try
            {
                p.Start();
                return p;
            }
            catch (Exception e)
            {
                base.Error = e.Message;
                return null; // return null if fail to start
            }
        }


        // - Stop server function
        public async Task Stop(Process p) => await Task.Run(() => { p.Kill(); }); // Fallback for Killing the server process, if the server doesn't support Stop() function'
    }
}