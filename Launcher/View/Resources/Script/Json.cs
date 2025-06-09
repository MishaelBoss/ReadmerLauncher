using Launcher.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace Launcher.View.Resources.Script
{
    public class CheckUpdate
    {
        public static async Task CheckUpdateJson()
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync(Arguments.urlJSONUpdateLauncher);
            var data = JsonConvert.DeserializeObject<UpdateLauncher>(json);

            Arguments.fileDownloadLink = data.fileDownloadLink;
            Arguments.newVersion = data.version;
            Arguments.aboutVersionLink = data.aboutVersionLink;
            Arguments.readver = data.version;
            Arguments.isEmergencyUpdate = data.isEmergencyUpdate;
        }
    }
}
