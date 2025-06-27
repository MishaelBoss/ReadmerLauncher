using Launcher.Model;
using System.IO;
using System.Text.Json;

namespace Launcher.View.Resources.Script.Cookie
{
    public static class ReaderCookie
    {
        public static bool IsUserLoggedIn(string username)
        {
            try {
                string cookieFilePath = Path.Combine(Paths.cookie(username), "login.cookie");

                if (!File.Exists(cookieFilePath))
                    return false;

                var json = File.ReadAllText(cookieFilePath);
                var data = JsonSerializer.Deserialize<CookieServer>(json);

                return data?.Expires > DateTime.Now;
            }
            catch {
                return false;
            }
        }
    }
}
