using Launcher.Model.Interface;
using Newtonsoft.Json;

namespace Launcher.Model
{
    public class CookieServer : ICookie
    {
        [JsonProperty("id")]
        public double id { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("Expires")]
        public DateTime Expires { get; set; }
    }
}
