using System.Net;

namespace Launcher.View.Resources.Script
{
    internal class Internet
    {
        public static bool connect() {
            try {
                Dns.GetHostEntry("dotnet.beget.tech");
                return true;
            }
            catch { 
                return false;
            }
        }

        public static void reconnect() {
            connect();
        }
    }
}
