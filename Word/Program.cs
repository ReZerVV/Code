using EasyUI.Draw;
using System.Diagnostics;
using Word.CustomElements;

internal class Program
{
    public static bool isOpen = true;
    private static DocumentEditor documentEditor = null;

    public static void Main(string[] argv)
    {
        documentEditor = new DocumentEditor 
        {
            Size = new EasyUI.Core.Vector2(60, 40)
        };
        AppLoop();
    }

    public static void AppLoop()
    {

        Canvas canvas = new Canvas(Console.WindowWidth, Console.WindowHeight, Color.Black);
        Console.SetBufferSize(canvas.Width, canvas.Height);
        Console.SetWindowSize(canvas.Width, canvas.Height);
        Stopwatch stopwatch = new Stopwatch();
        Thread consoleInputThread = new Thread(ConsoleInput);

        consoleInputThread.Start();
        while (isOpen)
        {
            stopwatch.Restart();
            {
                documentEditor.Render(canvas);
            }
            canvas.RenderBuffer();
            if (canvas.Width != Console.WindowWidth ||
                canvas.Height != Console.WindowHeight) 
            {
                canvas = new Canvas(Console.WindowWidth, Console.WindowHeight, Color.Black);
            }
            stopwatch.Stop();
            int timeRenderFrame = 8 - stopwatch.Elapsed.Milliseconds;
            if (timeRenderFrame > 0)
            {
                Thread.Sleep(timeRenderFrame);
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
                else if (keyInfo.Modifiers == ConsoleModifiers.Control &&
                    keyInfo.Key == ConsoleKey.I)
                {
                    documentEditor.IsFocus = !documentEditor.IsFocus;
                }
                else
                {
                    documentEditor.Update(keyInfo);
                }
            }
        }
    }
}