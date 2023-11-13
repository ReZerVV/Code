namespace Word.Commands
{
    public interface ICommand
    {
        string Name { get; }
        bool HasArgs { get; }
        int CountArgs { get; }
        List<string> Args { get; set; }

        List<string> Help();
        void Execute();
    }
}
