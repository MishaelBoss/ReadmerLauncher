using Launcher.Model.Interface;

namespace Launcher.Model
{
    public class Game : IGame
    {
        public double _id { get; set; }
        public string _name { get; set; }
        public string _version { get; set; }
        public string _background { get; set; }
        public string _icon { get; set; }
    }
}
