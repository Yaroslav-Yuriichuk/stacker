namespace Stacker.Services
{
    public interface IApplicationWindowsService
    {
        public void Open(WindowType window);
        public void Close(WindowType window);
    }

    public enum WindowType
    {
        HeightSettings
    }
}
