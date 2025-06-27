using Newtonsoft.Json;

namespace Launcher.Model.Interface
{
    public interface IUpdateLauncher
    {
        [JsonProperty("version")]
        public string version { get; set; }

        [JsonProperty("isEmergencyUpdate")]
        public bool isEmergencyUpdate { get; set; }

        [JsonProperty("fileDownloadLink")]
        public string fileDownloadLink { get; set; }

        [JsonProperty("aboutVersionLink")]
        public string aboutVersionLink { get; set; }
    }
}