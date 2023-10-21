namespace Word.Core.Test 
{
    internal class Program 
    {
        public static void Main(string[] args)
        {
            Document document = Document.SampleDocument();
            Console.CursorVisible = false;
            while (true)
            {
                PrindDocument(DocumentToStringLines(document));
            }
        }

        public static List<string> DocumentToStringLines(Document document)
        {
            List<string> documentLines = new List<string>();
            int lineIndex = -1;
            foreach (DocumentNode documentNode in document)
            {
                switch (documentNode.Type)
                {
                    case DocumentNodeType.Paragraph:
                        {
                            documentLines.Add(string.Empty);
                            lineIndex += 1;
                        }
                        break;

                    case DocumentNodeType.Span:
                        {
                            foreach (string word in documentNode.InnerText.Split(' '))
                            {
                                if (documentLines[lineIndex].Length + word.Length >= 80)
                                {
                                    lineIndex++;
                                    documentLines.Add(string.Empty);
                                }
                                documentLines[lineIndex] += word;
                            }
                        }
                        break;
                }
            }
            return documentLines;
        }

        public static void PrindDocument(List<string> documentLines)
        {
            foreach (string documentLine in documentLines)
            {
                Console.WriteLine(documentLine);
            }
            Thread.Sleep(16);
            Console.Clear();
        }
    }
}