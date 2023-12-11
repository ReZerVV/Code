using System.Data;
using System.Reflection;
using Word.Commands;

namespace Word.Services
{
    public class CommandService
    {
        public static List<ICommand> Commands => GetCommands();

        public static List<ICommand> GetCommands()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => (ICommand)Activator.CreateInstance(type))
                .ToList();
        }

        public List<string> Search(string commandName)
        {
            return Commands
                .Where(command => command.Name.ToUpper().Contains(commandName.ToUpper()))
                .Select(command => command.Name)
                .ToList();
        }

        public ICommand GetByName(string commandName)
        {
            return Commands
                .Where(command => command.Name.ToUpper().Contains(commandName.ToUpper()))
                .Single();
        }
    }
}
