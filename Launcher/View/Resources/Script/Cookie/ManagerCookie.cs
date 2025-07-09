using Launcher.Model;
using System.IO;
using System.Text.Json;

namespace Launcher.View.Resources.Script.Cookie
{
    public class ManagerCookie
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

        public static bool IsUserLoggedIn(string username)
        {
            try
            {
                string cookieFilePath = Path.Combine(Paths.cookie(username), "login.cookie");

                if (!File.Exists(cookieFilePath))
                    return false;

                var json = File.ReadAllText(cookieFilePath);
                var data = JsonSerializer.Deserialize<CookieServer>(json);

                return data?.Expires > DateTime.Now;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteCookie(string username)
        {
            string test = Path.Combine(Paths.cookie(username), "login.cookie");

            if (File.Exists(test))
                File.Delete(test);
        }
    }
}
