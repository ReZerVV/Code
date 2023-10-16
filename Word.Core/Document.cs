namespace Word.Core
{
    public class Document
    {
        public DocumentNode Body { get; set; }
        public string Name => Body.Content; 

        public Document(string name)
        {
            Body = new DocumentNode();
            Body.Content = name;
        }


    }
}
