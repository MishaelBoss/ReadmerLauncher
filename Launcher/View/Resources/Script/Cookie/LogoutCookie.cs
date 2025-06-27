using System.IO;

namespace Launcher.View.Resources.Script.Cookie
{
    public class LogoutCookie
    {
        public static void DeleteCookie(string username)
        {
            string test = Path.Combine(Paths.cookie(username), "login.cookie");

            if (File.Exists(test))
                File.Delete(test);
        }
    }
}
