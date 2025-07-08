using Launcher.Model.Interface;

namespace Launcher.Model
{
    public class User : IUser
    {
        public double id { get; set; }
        public string Username { get; set; }
        public string avatar { get; set; }
        public string email { get; set; }
        public string number_phone { get; set; }
        public decimal money { get; set; }
        public string location { get; set; }
    }
}
