namespace Launcher.Model
{
    internal class UpdateLauncher : IUpdateLauncher
    {
        public string version { get; set; }
        public string fileDownloadLink { get; set; }
    }
}