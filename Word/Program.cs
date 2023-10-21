using EasyUI.Draw;
using System.Diagnostics;
using Word.Core;
using Word.CustomElements;

internal class Program
{
    public static bool isOpen = true;
    private static DocumentViewer documentViewer = null;
    private static Canvas canvas;

    protected static int FPS;
    protected static bool isOpenDebugInfo;
    private static System.Timers.Timer fpsTimer;
    private static Stopwatch fpsSynchronizationTimer;
    private static int fps;
    private static float frt;
    private static List<int> frtHistory = new List<int>();
    private static List<int> fpsHistory = new List<int>();

    public static void Main(string[] argv)
    {
        documentViewer = new DocumentViewer
        {
            Position = new EasyUI.Core.Vector2(1, 1),
            Size = new EasyUI.Core.Vector2(110, 40),
            Document = Document.SampleDocument(),
        };
        isOpen = true;
        isOpenDebugInfo = false;
        FPS = 60;
        fpsTimer = new System.Timers.Timer(1000);
        fpsSynchronizationTimer = new Stopwatch();
        canvas = new Canvas(Console.WindowWidth, Console.WindowHeight, Color.Black);
        AppLoop();
    }

    public static void AppLoop()
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
        while (isOpen)
        {
            fpsSynchronizationTimer.Restart();
            {
                Update();
                Layout();
                Render();
            }
            if (isOpenDebugInfo)
            {
                DrawInfo();
            }
            canvas.RenderBuffer();
            fpsSynchronizationTimer.Stop();
            
            if (canvas.Width != Console.WindowWidth ||
                canvas.Height != Console.WindowHeight)
            {
                canvas = new Canvas(Console.WindowWidth, Console.WindowHeight, Color.Black);
            }
            
            fpsCounter++;
            frt = (float)fpsSynchronizationTimer.Elapsed.TotalMilliseconds;
            frtHistory.Add((int)frt / 20);

            int timeSynchronizationSleep = (int)(0.5f / FPS * 1000.0f - fpsSynchronizationTimer.Elapsed.TotalMilliseconds);
            if (timeSynchronizationSleep > 0)
            {
                Thread.Sleep(timeSynchronizationSleep);
            }

            canvas.ClearBuffer();
        }
        consoleInputThread.Join();
    }

    public static void ConsoleInput()
    {
        while (isOpen)
        {
            if (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isOpen = false;
                }
                if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.I)
                {
                    isOpenDebugInfo = !isOpenDebugInfo;
                }
                documentViewer.Input(keyInfo);
            }
        }
    }

    public static void DrawInfo() 
    {
        for (int colPosition = 0, index = fpsHistory.Count >= 40 ? fpsHistory.Count - 40 : 0;
            index < fpsHistory.Count;
            colPosition++, index++)
        {
            if (fpsHistory[colPosition] > 0)
            {
                canvas.DrawLine(
                    x0: colPosition,
                    y0: 23 - fpsHistory[colPosition],
                    x1: colPosition,
                    y1: 23,
                    new Color(24, 61, 61));
                canvas.DrawLine(
                    x0: colPosition,
                    y0: 23 - fpsHistory[colPosition],
                    x1: colPosition,
                    y1: 23 - fpsHistory[colPosition],
                    new Color(92, 131, 116));
            }
            else
            {
                canvas.DrawLine(
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
                canvas.DrawLine(
                    x0: colPosition,
                    y0: 13 - frtHistory[colPosition],
                    x1: colPosition,
                    y1: 13,
                    new Color(39, 55, 77));
                canvas.DrawLine(
                    x0: colPosition,
                    y0: 13 - frtHistory[colPosition],
                    x1: colPosition,
                    y1: 13 - frtHistory[colPosition],
                    new Color(157, 178, 191));
            }
            else
            {
                canvas.DrawLine(
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
        canvas.DrawText($"  Frames per second: {fps}", 0, 0, Color.White, new Color(50, 50, 100));
        canvas.DrawText($"*", 0, 0, new Color(119, 228, 212), new Color(50, 50, 100));
        canvas.DrawText($"  Frame rendering time: {frt:F2}", 0, 1, Color.White, new Color(50, 50, 100));
        canvas.DrawText($"*", 0, 1, new Color(180, 254, 152), new Color(50, 50, 100));
        canvas.DrawText($"Window size: {Console.WindowWidth}:{Console.WindowHeight}", 0, 2, Color.White, new Color(50, 50, 100));
    }

    public static void Update()
    {
        documentViewer.Update();
    }

    public static void Layout()
    {
        documentViewer.Position = new EasyUI.Core.Vector2(
            (canvas.Width - documentViewer.Size.x) / 2,
            (canvas.Height - documentViewer.Size.y) / 2);
        documentViewer.Layout();
    }

    public static void Render()
    {
        documentViewer.Render(canvas);
    }   
}