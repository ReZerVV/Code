using System.Reflection;
using Word.Commands;

namespace Word.Services
{
    public class CommandService
    {
        public List<string> Search(string commandName)
        {
            var commandTypes = Assembly.GetExecutingAssembly().GetTypes();
            var commandImplementations = commandTypes
                .Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);
            var commands = new List<ICommand>();
            foreach (var commandType in commandImplementations)
            {
                commands.Add((ICommand)Activator.CreateInstance(commandType));
            }
            return commands
                .Where(command => command.Name.ToUpper().Contains(commandName.ToUpper()))
                .Select(command => command.Name)
                .ToList();
        }

        public ICommand GetByName(string commandName)
        {
            var commandTypes = Assembly.GetExecutingAssembly().GetTypes();
            var commandImplementations = commandTypes
                .Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);
            var commands = new List<ICommand>();
            foreach (var commandType in commandImplementations)
            {
                commands.Add((ICommand)Activator.CreateInstance(commandType));
            }
            return commands
                .Where(command => command.Name.ToUpper().Contains(commandName.ToUpper()))
                .Single();
        }
    }
}
