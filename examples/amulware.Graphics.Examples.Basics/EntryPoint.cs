using OpenTK;

namespace amulware.Graphics.Examples.Basics
{
    static class EntryPoint
    {
        public static void Main()
        {
            // Initialize the OpenTK toolkit. This is an optional step, unless you use a third-party windowing system
            // (such as GTK#). However, initializing the toolkit explicitly allows us to specify the options with which
            // OpenTK is initialized. Specifically, we use the PreferNative backend, which will prefer the native
            // windowing system over the SDL2 one in macOS, as the latter tends to expose some graphics bugs.
            using (Toolkit.Init(new ToolkitOptions {Backend = PlatformBackend.PreferNative}))
            {
                // Initialize an instance of the game window. This represents the game or application at the highest
                // level, and will be the entry point for your game loop.
                var gameWindow = new GameWindow();
                // Run the game window. We can optionally specify target FPS and UPS.
                gameWindow.Run();
            }
        }
    }
}
