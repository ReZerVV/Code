using System.IO;
using System.Runtime.CompilerServices;

namespace Word.Core
{
    // Helper for loading document
    public static class DocumentLoader
    {
        public static Document LoadFilePart(string path, int start, int count)
        {
            // Create and init path property for new document
            Document doc = new Document();
            doc.SetPath(path);

            // Checking the existence of a file along a path
            if (!File.Exists(doc.GetPath()))
            {
                throw new FileNotFoundException();
            }
            using StreamReader fileStreamReader = new StreamReader(doc.GetPath());

            // Update properties
            doc.Clear();
            doc.Encoding = fileStreamReader.CurrentEncoding;

            // Skip file lines before start position
            for (int lineIndex = 0; lineIndex < start; lineIndex++)
            {
                if (!TryReadLine(fileStreamReader, out string tempLine))
                {
                    throw new ArgumentOutOfRangeException(nameof(start));
                }
            }

            // Read lines from file
            for (int lineIndex = 0; lineIndex < count; lineIndex++)
            {
                if (TryReadLine(fileStreamReader, out string tempLine))
                {
                    doc.Buffer.Add(tempLine);
                }
                else
                {
                    break;
                }
            }

            return doc;
        }

        public static bool TryLoadFilePart(Document doc, int start, int count)
        {
            try
            {
                if (!File.Exists(doc.GetPath()))
                {
                    throw new FileNotFoundException();
                }
                using StreamReader fileStreamReader = new StreamReader(doc.GetPath());

                // Update properties
                doc.Clear();
                doc.Encoding = fileStreamReader.CurrentEncoding;

                // Skip file lines before start position
                for (int lineIndex = 0; lineIndex < start; lineIndex++)
                {
                    if (!TryReadLine(fileStreamReader, out string tempLine))
                    {
                        throw new ArgumentOutOfRangeException(nameof(start));
                    }
                }

                // Read lines from file
                for (int lineIndex = 0; lineIndex < count; lineIndex++)
                {
                    if (TryReadLine(fileStreamReader, out string tempLine))
                    {
                        doc.Buffer.Add(tempLine);
                    }
                    else
                    {
                        break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryLoadFilePart(string path, int start, int count, out Document doc)
        {
            try
            {
                // Create and init path property for new document
                doc = new Document();
                doc.SetPath(path);

                // Checking the existence of a file along a path
                if (!File.Exists(doc.GetPath()))
                {
                    throw new FileNotFoundException();
                }
                using StreamReader fileStreamReader = new StreamReader(doc.GetPath());

                // Update properties
                doc.Clear();
                doc.Encoding = fileStreamReader.CurrentEncoding;

                // Skip file lines before start position
                for (int lineIndex = 0; lineIndex < start; lineIndex++)
                {
                    if (!TryReadLine(fileStreamReader, out string tempLine))
                    {
                        throw new ArgumentOutOfRangeException(nameof(start));
                    }
                }

                // Read lines from file
                for (int lineIndex = 0; lineIndex < count; lineIndex++)
                {
                    if (TryReadLine(fileStreamReader, out string tempLine))
                    {
                        doc.Buffer.Add(tempLine);
                    }
                    else
                    {
                        break;
                    }
                }

                return true;
            }
            catch
            {
                // Set the doc variable to null
                doc = null;

                return false;
            }
        }

        private static bool TryReadLine(StreamReader fileStreamReader, out string line)
        {
            return (line = fileStreamReader.ReadLine()) != null;
        }

        public static int GetLineCountFromFile(string path)
        {
            // Checking the existence of a file along a path
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using StreamReader fileStreamReader = new StreamReader(path);

            // Calc line count
            int size = 0;
            for (; TryReadLine(fileStreamReader, out string tempLine); size++) { }

            return size;
        }

        public static int GetLineCountFromFile(Document doc)
        {
            // Checking the existence of a file along a path
            if (!File.Exists(doc.GetPath()))
            {
                throw new FileNotFoundException();
            }

            using StreamReader fileStreamReader = new StreamReader(doc.GetPath());
            
            
            // Calc line count
            int size = 0;
            for (; TryReadLine(fileStreamReader, out string tempLine); size++) { }

            return size;
        }

        public static bool TryGetLineCountFromFile(Document doc, out int size)
        {
            // Checking the existence of a file along a path
            if (!File.Exists(doc.GetPath()))
            {
                size = 0;
                return false;
            }

            using StreamReader fileStreamReader = new StreamReader(doc.GetPath());


            // Calc line count
            size = 0;
            for (; TryReadLine(fileStreamReader, out string tempLine); size++) { }

            return true;
        }

        public static bool TrySave(Document doc, string path)
        {
            try
            {
                doc.SetPath(path);
                using StreamWriter streamWriter = new StreamWriter(
                    stream: new FileStream(doc.GetPath(), FileMode.OpenOrCreate,  FileAccess.Write),
                    encoding: doc.Encoding);
                foreach (string line in doc.Buffer)
                {
                    streamWriter.WriteLine(line);
                }
                doc.IsSaved = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TrySave(Document doc)
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

                using StreamReader streamReader = new StreamReader(doc.GetPath());
                using StreamWriter streamWriter = new StreamWriter(doc.GetPath());
                int totalLineCount = doc.GetTotalLineCount();

                { 
                    for (int lineIndex = 0; lineIndex < doc.PartOffset; lineIndex++)
                    {
                        string? line;
                        if ((line = streamReader.ReadLine()) != null)
                        {
                            streamWriter.WriteLine(line);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                { 
                    for (int lineIndex = 0; lineIndex < doc.Buffer.Count; lineIndex++)
                    {
                        streamWriter.WriteLine(doc.Buffer[lineIndex - doc.PartOffset]);
                        streamReader.ReadLine();
                    }
                }

                { 
                    string? line;
                    for (; (line = streamReader.ReadLine()) != null; )
                    {
                        streamWriter.WriteLine(line);
                    }
                }

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
