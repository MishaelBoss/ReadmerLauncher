namespace Launcher.Model.Interface
{
    public interface IUser
    {
        double id { get; set; }
        string Username { get; set; }
        string avatar { get; set; }
        string email { get; set; }
        string number_phone { get; set; }
        decimal money { get; set; }
        string location { get; set; }
    }
}
