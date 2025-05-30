using Newtonsoft.Json;

namespace Launcher.Model
{
    public interface IUpdateLauncher
    {
        [JsonProperty("version")]
        public string version { get; set; }

        [JsonProperty("fileDownloadLink")]
        public string fileDownloadLink { get; set; }
    }
}