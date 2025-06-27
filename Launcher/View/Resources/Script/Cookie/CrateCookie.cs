using System.IO;
using System.Text.Json;

namespace Launcher.View.Resources.Script.Cookie
{
    public static class CrateCookie
    {
        public static void SaveLoginCookie(string username, string token, DateTime expires)
        {
            var data = new
            {
                Username = username,
                Token = token,
                Expires = expires
            };

/*            string userDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Assembly.GetEntryAssembly().GetName().Name,
                "userdata",
                username
            );*/

            string cookieFilePath = Path.Combine(Paths.cookie(username), "login.cookie");
            File.WriteAllText(cookieFilePath, JsonSerializer.Serialize(data));
        }
    }
}
