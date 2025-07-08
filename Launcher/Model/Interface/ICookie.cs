namespace Launcher.Model.Interface
{
    public interface ICookie
    {
        double id { get; set; }
        string Username { get; set; }
        string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
