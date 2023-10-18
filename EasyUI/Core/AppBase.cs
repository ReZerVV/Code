using System.Diagnostics;
using EasyUI.Draw;

namespace EasyUI.Core
{
    public abstract class AppBase
    {
        protected Canvas MainCanvas;
        public static WindowBase? MainWindow = null;
        public static List<WindowBase> Frames { get; set; } = new List<WindowBase>();
        public static List<string> Errors = new List<string>();
        
        protected readonly int FPS;
        protected bool isOpenDebugInfo;
        private System.Timers.Timer fpsTimer;
        private Stopwatch fpsSynchronizationTimer;
        private int fps;
        private float frt;

        public bool IsOpen { get; private set; }

        public AppBase(int width, int height, string title = "App (cyril project)")
        {
            Console.Title = title;
            MainCanvas = new Canvas(width, height, Color.White);
            if (MainWindow != null)
            {
                MainWindow.Size.x = MainCanvas.Width;
                MainWindow.Size.y = MainCanvas.Height;
            }
            IsOpen = true;
            isOpenDebugInfo = false;
            FPS = 60;
            fpsTimer = new System.Timers.Timer(1000);
            fpsSynchronizationTimer = new Stopwatch();
        }

        public void AppLoop()
        {
            Thread consoleInputThread = new Thread(ConsoleInput);
            if (MainWindow == null)
            {
                Errors.Add($"MainWindow is null, no content to display");
            }
            int fpsCounter = 0;
            fpsTimer.Elapsed += (sender, e) =>
            {
                fps = fpsCounter;
                fpsCounter = 0;
            };

            consoleInputThread.Start();
            fpsTimer.Start();

            while (IsOpen)
            {
                fpsSynchronizationTimer.Restart();
                {
                    OnUpdate();
                    OnLateUpdate();
                    OnRender();
                }
                if (isOpenDebugInfo)
                {
                    MainCanvas.DrawText($"Frames per second: {fps}", 0, 0, Color.White, new Color(50, 50, 100));
                    MainCanvas.DrawText($"Frame rendering time: {frt:F2}", 0, 1, Color.White, new Color(50, 50, 100));
                    MainCanvas.DrawText($"Window size: {Console.WindowWidth}:{Console.WindowHeight}", 0, 2, Color.White, new Color(50, 50, 100));
                    if (Errors.Count > 0)
                    {
                        MainCanvas.DrawText($"[Errors]", 0, 3, Color.Red, new Color(50, 50, 100));
                        for (int indexError = 0, y = 4; indexError < Errors.Count; indexError++, y++)
                        {
                            MainCanvas.DrawText($" [{indexError + 1}] {Errors[indexError]}", 0, y, Color.Red, new Color(50, 50, 100));
                        }
                    }
                }
                MainCanvas.RenderBuffer();
                fpsSynchronizationTimer.Stop();

                fpsCounter++;
                frt = (float)fpsSynchronizationTimer.Elapsed.TotalMilliseconds;

                if (MainCanvas.Width != Console.WindowWidth ||
                    MainCanvas.Height != Console.WindowHeight)
                {
                    MainCanvas = new Canvas(Console.WindowWidth, Console.WindowHeight, Color.White);
                    if (MainWindow != null)
                    {
                        MainWindow.Size.x = MainCanvas.Width;
                        MainWindow.Size.y = MainCanvas.Height;
                    }
                }

                int timeSynchronizationSleep = (int)(0.5f / FPS * 1000.0f - fpsSynchronizationTimer.Elapsed.TotalMilliseconds);
                if (timeSynchronizationSleep > 0)
                {
                    Thread.Sleep(timeSynchronizationSleep);
                }

                MainCanvas.ClearBuffer();
            }
            OnExit();
            fpsTimer.Stop();
            consoleInputThread.Join();
        }

        private void ConsoleInput()
        {
            while (IsOpen)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        IsOpen = false;
                    }
                    if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.I)
                    {
                        isOpenDebugInfo = !isOpenDebugInfo;
                    }
                    OnInput(keyInfo);
                }
            }
        }

        protected virtual void OnExit()
        {

        }

        protected virtual void OnInput(ConsoleKeyInfo keyInfo)
        {
            if (MainWindow != null)
            {
                MainWindow.Input(keyInfo);
            }
            foreach (var frame in Frames)
            {
                frame.Input(keyInfo);
            }
        }

        protected virtual void OnRender()
        {
            if (MainWindow != null)
            {
                MainWindow.Render(MainCanvas);
            }
            foreach (var frame in Frames)
            {
                frame.Render(MainCanvas);
            }
        }

        protected virtual void OnUpdate()
        {
            if (MainWindow != null)
            {
                MainWindow.Update();
            }
            foreach (var frame in Frames)
            {
                frame.Update();
            }
        }

        protected virtual void OnLateUpdate()
        {
            if (MainWindow != null)
            {
                MainWindow.LateUpdate();
            }
            foreach (var frame in Frames)
            {
                frame.LateUpdate();
            }
        }
    }
}