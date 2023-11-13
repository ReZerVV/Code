using System.Diagnostics;
using EasyUI.Core.Components;
using EasyUI.Draw;

namespace EasyUI.Core
{
    public abstract class AppBase
    {
        protected Canvas MainCanvas { get; set; }
        protected IComponent MainComponent { get; set; }
        protected int FPS;
        protected bool IsOpenDebugInfo { get; set; }
        protected List<string> Errors { get; set; } = new();

        private System.Timers.Timer fpsTimer;
        private Stopwatch fpsSynchronizationTimer;
        private int fps;
        private List<int> fpsHistory = new();
        private float frt;
        private List<int> frtHistory = new();

        private Queue<ConsoleKeyInfo> eventQueue = new();

        public bool IsOpen { get; private set; }

        public AppBase(string title = "App (cyril project)")
        {
            Console.Title = title;
            Initialize();
        }

        public void Initialize()
        {
            MainCanvas = new Canvas(Console.WindowWidth, Console.WindowHeight, Color.Black);
            MainComponent = null;
            IsOpen = true;
            IsOpenDebugInfo = false;
            FPS = 60;
            fpsTimer = new System.Timers.Timer(1000);
            fpsSynchronizationTimer = new Stopwatch();
        }

        private void DrawInfoDebugMenu()
        {
            for (int colPosition = 0, index = fpsHistory.Count >= 40 ? fpsHistory.Count - 40 : 0;
            index < fpsHistory.Count;
            colPosition++, index++)
            {
                if (fpsHistory[colPosition] > 0)
                {
                    MainCanvas.DrawLine(
                        x0: colPosition,
                        y0: 23 - fpsHistory[colPosition],
                        x1: colPosition,
                        y1: 23,
                        new Color(24, 61, 61));
                    MainCanvas.DrawLine(
                        x0: colPosition,
                        y0: 23 - fpsHistory[colPosition],
                        x1: colPosition,
                        y1: 23 - fpsHistory[colPosition],
                        new Color(92, 131, 116));
                }
                else
                {
                    MainCanvas.DrawLine(
                        x0: colPosition,
                        y0: 23,
                        x1: colPosition,
                        y1: 23,
                        new Color(4, 13, 18));
                }
            }
            if (fpsHistory.Count >= 40)
            {
                fpsHistory.RemoveRange(0, fpsHistory.Count - 40);
            }
            for (int colPosition = 0, index = frtHistory.Count >= 40 ? frtHistory.Count - 40 : 0;
                index < frtHistory.Count;
                colPosition++, index++)
            {
                if (frtHistory[colPosition] > 0)
                {
                    MainCanvas.DrawLine(
                        x0: colPosition,
                        y0: 13 - frtHistory[colPosition],
                        x1: colPosition,
                        y1: 13,
                        new Color(39, 55, 77));
                    MainCanvas.DrawLine(
                        x0: colPosition,
                        y0: 13 - frtHistory[colPosition],
                        x1: colPosition,
                        y1: 13 - frtHistory[colPosition],
                        new Color(157, 178, 191));
                }
                else
                {
                    MainCanvas.DrawLine(
                        x0: colPosition,
                        y0: 13,
                        x1: colPosition,
                        y1: 13,
                        new Color(0, 21, 36));
                }
            }
            if (frtHistory.Count >= 40)
            {
                frtHistory.RemoveRange(0, frtHistory.Count - 40);
            }
            MainCanvas.DrawText($"Frames per second: {fps}", 0, 0, Color.White, new Color(50, 50, 100), PixelStyle.StyleNone);
            MainCanvas.DrawText($"Frame rendering time: {frt:F2}", 0, 1, Color.White, new Color(50, 50, 100), PixelStyle.StyleNone);
            MainCanvas.DrawText($"Window size: {Console.WindowWidth}:{Console.WindowHeight}", 0, 2, Color.White, new Color(50, 50, 100), PixelStyle.StyleNone);
        }

        public void AppLoop()
        {
            Thread consoleInputThread = new Thread(ConsoleInput);
            int fpsCounter = 0;
            fpsTimer.Elapsed += (sender, e) =>
            {
                fps = fpsCounter;
                fpsHistory.Add((int)fps / 10);
                fpsCounter = 0;
            };

            consoleInputThread.Start();
            fpsTimer.Start();

            while (IsOpen)
            {
                fpsSynchronizationTimer.Restart();
                {
                    if (eventQueue.TryDequeue(out ConsoleKeyInfo keyInfo))
                    {
                        OnInput(keyInfo);
                    }
                    OnUpdate();
                    OnLayout();
                    OnRender();
                }
                if (IsOpenDebugInfo)
                {
                    DrawInfoDebugMenu();
                }
                MainCanvas.RenderBuffer();
                fpsSynchronizationTimer.Stop();

                fpsCounter++;
                frt = (float)fpsSynchronizationTimer.Elapsed.TotalMilliseconds;
                frtHistory.Add((int)frt / 5);

                if (MainCanvas.Width != Console.WindowWidth ||
                    MainCanvas.Height != Console.WindowHeight)
                {
                    MainCanvas = new Canvas(Console.WindowWidth, Console.WindowHeight, Color.Black);
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
                    if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.X)
                        IsOpen = false;
                    else if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.I)
                        IsOpenDebugInfo = !IsOpenDebugInfo;
                    else
                        eventQueue.Enqueue(keyInfo);
                }
            }
        }

        protected virtual void OnExit()
        {

        }

        protected virtual void OnInput(ConsoleKeyInfo keyInfo)
        {
            if (MainComponent != null)
            {
                MainComponent.Input(keyInfo);
            }
        }

        protected virtual void OnUpdate()
        {
            if (MainComponent != null)
            {
                MainComponent.Update();
            }
        }

        protected virtual void OnRender()
        {
            if (MainComponent != null)
            {
                MainComponent.Render(MainCanvas);
            }
        }

        protected virtual void OnLayout()
        {
            if (MainComponent != null)
            {
                MainComponent.Size = new Vector2(MainCanvas.Width, MainCanvas.Height);
                MainComponent.Layout();
            }
        }
    }
}