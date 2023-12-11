namespace Word.Core
{
    // Helper for loading document
    public static class DocumentLoader
    {
        public static bool TryLoad(Document doc)
        {
            try
            {
                if (!doc.HasPath())
                {
                    return false;
                }

                if (!File.Exists(doc.GetPath()))
                {
                    return false;
                }

                doc.Buffer.Clear();

                using StreamReader streamReader = new StreamReader(doc.GetPath());
                while (TryReadLine(streamReader, out string line))
                    doc.Buffer.Add(line);

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public static bool TryLoad(string path, out Document doc)
        {
            doc = new Document();
            doc.SetPath(path);
            return TryLoad(doc);
        }

        private static bool TryReadLine(StreamReader fileStreamReader, out string line)
        {
            return (line = fileStreamReader.ReadLine()) != null;
        }

        public static bool TrySave(Document doc)
        {
            try
            {
                if (!doc.HasPath())
                {
                    return false;
                }

                if (File.Exists(doc.GetPath()))
                {
                    File.Delete(doc.GetPath());
                }

                using StreamWriter streamWriter = new StreamWriter(doc.GetPath());
                
                foreach (string line in doc.Buffer)
                    streamWriter.WriteLine(line);
                
                doc.IsSaved = true;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
