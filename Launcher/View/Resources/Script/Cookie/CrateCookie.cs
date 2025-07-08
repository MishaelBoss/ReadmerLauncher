using System.IO;
using System.Text.Json;

namespace Launcher.View.Resources.Script.Cookie
{
    public static class CrateCookie
    {
        public static void SaveLoginCookie(double id, string username, string token, DateTime expires)
        {
            var data = new
            {
                id = id,
                Username = username,
                Token = token,
                Expires = expires
            };

            string cookieFilePath = Path.Combine(Paths.cookie(username), "login.cookie");
            File.WriteAllText(cookieFilePath, JsonSerializer.Serialize(data));
        }
    }
}
