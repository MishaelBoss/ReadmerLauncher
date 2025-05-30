namespace LauncherLes1.Model
{
    public interface IServer
    {
        string Name { get; set; }
        string Version { get; set; }
        public Platform Platform { get; set; }
        public string Download { get; set; }
        public string Icon { get; set; }
        public string Background { get; set; }
    }
}
