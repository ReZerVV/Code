using System.Reflection;

namespace Word.Core.Syntax.Styles
{
    public static class MarkerService
    {
        public static List<IMarker> Markers => GetMarkers();

        public static List<IMarker> GetMarkers()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(IMarker).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => (IMarker)Activator.CreateInstance(type))
                .ToList();
        }
    }

    public interface IMarker
    {
        public string LangName { get; }
        public string LangExtension { get; }
        public List<List<RichText>> Markup(List<string> lines);
    }
}
