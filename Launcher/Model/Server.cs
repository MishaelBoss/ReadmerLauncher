using Newtonsoft.Json;

namespace LauncherLes1.Model
{
    internal class Server : IServer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("platform")]
        public Platform Platform { get; set; }

        [JsonProperty("download")]
        public string Download { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("backround")]
        public string Background { get; set; }
    }

    public class Platform
    {
        [JsonProperty("windows")]
        public bool Windows { get; set; }

        [JsonProperty("linux")]
        public bool Linux { get; set; }

        [JsonProperty("mac")]
        public bool Mac { get; set; }
    }
}
