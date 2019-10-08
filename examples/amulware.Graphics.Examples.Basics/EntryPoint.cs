using OpenTK;

namespace amulware.Graphics.Examples.Basics
{
    static class EntryPoint
    {
        public static void Main()
        {
            using (Toolkit.Init(new ToolkitOptions {Backend = PlatformBackend.PreferNative}))
            {
                var gameWindow = new GameWindow();
                gameWindow.Run();
            }
        }
    }
}
