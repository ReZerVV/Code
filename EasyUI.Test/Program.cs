using EasyUI.Core;

internal class Program : AppBase
{
    public static void Main(string[] argv)
    {
        new Program();
    }

    public Program(string title = "App (cyril project)")
        : base(title)
    {
        AppLoop();
    }


}