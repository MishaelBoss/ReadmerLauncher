namespace Launcher.Model.Interface
{
    public interface ICookie
    {
        string Username { get; set; }
        string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
