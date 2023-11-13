using System.Reflection;
using System.Text.RegularExpressions;
using Word.Core.Syntax;
using Word.Stores;

namespace Word.Core
{
    public class Document
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<string> Lines { get; set; }
        public bool IsSaved { get; set; }

        public IHighlighter Highlighter { get; set; }

        public Document(string name)
        {
            Lines = new List<string>();
            Lines.Add(string.Empty);
            IsSaved = false;
            Name = name;
            Path = string.Empty;
            Highlighter = GetHighlighterByExtension(Name);
        }

        public List<List<TextFragmentInfo>> GetMarkupText()
        {
            List<List<TextFragmentInfo>> markupText = new();
            foreach (string line in Lines)
            {
                markupText.Add(Highlighter.Highlight(line));
            }
            return markupText;
        }

        public void Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    ApplicationCode.NotificationStore.Send(Notification.Error($"The document was not loaded"));
                    return;
                }
                Name = System.IO.Path.GetFileName(path);
                Path = System.IO.Path.GetDirectoryName(path);
                Highlighter = GetHighlighterByExtension(Name);
                Lines.Clear();
                using (StreamReader fileStream = new StreamReader(path))
                {
                    while (true)
                    {
                        string tempLine = fileStream.ReadLine();
                        if (tempLine == null)
                        {
                            break;
                        }
                        Lines.Add(tempLine);
                    }
                }
                IsSaved = true;
            }
            catch
            {
                ApplicationCode.NotificationStore.Send(Notification.Error($"There was an error while downloading the file"));
            }
        }

        private static IHighlighter GetHighlighterByExtension(string fileName)
        {
            var highlighter = (IHighlighter?)Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(IHighlighter).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => (IHighlighter)Activator.CreateInstance(type))
                .FirstOrDefault(highlighter => Regex.IsMatch(System.IO.Path.GetExtension(fileName), highlighter.LangExtension));
            if (highlighter == null)
            {
                return new TextPlainSyntaxHighlighter();
            }
            return highlighter;
        }

        public void Save()
        {
            try
            {
                using (StreamWriter outputFileStream = new StreamWriter(System.IO.Path.Combine(Path, Name)))
                {
                    foreach (string line in Lines)
                    {
                        outputFileStream.WriteLine(line);
                    }
                }
                IsSaved = true;
            }
            catch
            {
                ApplicationCode.NotificationStore.Send(Notification.Error($"The {Name} document was not saved"));
            }
        }
    }
}