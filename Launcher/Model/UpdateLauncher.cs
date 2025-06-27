using Launcher.Model.Interface;

namespace Launcher.Model
{
    internal class UpdateLauncher : IUpdateLauncher
    {
        public string version { get; set; }
        public bool isEmergencyUpdate { get; set; }
        public string fileDownloadLink { get; set; }
        public string aboutVersionLink { get; set; }
    }
}