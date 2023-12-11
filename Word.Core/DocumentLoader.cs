using Code.Core;

namespace Word.Core
{
    // Helper for loading document
    public static class DocumentLoader
    {
        public static int GetCountLinesFromFile(Document doc)
        {
            try
            {
                int lineCount = 0;

                using StreamReader streamReader = new StreamReader(doc.PathFrom);

                {
                    for (int lineIndex = 0; lineIndex < doc.PartOffset; lineIndex++, lineCount++)
                    {
                        if (!TryReadLine(streamReader, out string line))
                        {
                            throw new OutOfMemoryException();
                        }
                    }
                }

                {
                    foreach (DocumentPart docPart in doc.PartBuffer)
                    {
                        for (int lineIndex = 0; lineIndex < docPart.Buffer.Count; lineIndex++, lineCount++)
                        {
                            if (lineIndex < docPart.SizePart && !TryReadLine(streamReader, out string tempLine))
                            {
                                throw new OutOfMemoryException();
                            }
                        }
                    }
                }

                {
                    while (TryReadLine(streamReader, out string line))
                    {
                        lineCount++;
                    }
                }

                return lineCount;
            }
            catch
            {
                return 0;
            }
        }
        public const int PartSize = 50;
        public static Document LoadFilePart(string path, int start, int count)
        {
            // Create and init path property for new document
            Document doc = new Document();
            doc.SetPath(path);
            doc.IsLoadFromFile = true;
            doc.PathFrom = path;

            // Checking the existence of a file along a path
            if (!File.Exists(doc.GetPath()))
            {
                throw new FileNotFoundException();
            }
            using StreamReader fileStreamReader = new StreamReader(doc.PathFrom);

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
                if (lineIndex % PartSize == 0)
                {
                    doc.PartBuffer.Add(new DocumentPart(start));
                }
                if (TryReadLine(fileStreamReader, out string tempLine))
                {
                    doc.PartBuffer.Last().Buffer.Add(tempLine);
                    doc.PartBuffer.Last().SizePart++;
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
                doc.IsLoadFromFile = true;
                if (!File.Exists(doc.GetPath()))
                {
                    throw new FileNotFoundException();
                }
                using StreamReader fileStreamReader = new StreamReader(doc.PathFrom);

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
                    if (lineIndex % PartSize == 0)
                    {
                        doc.PartBuffer.Add(new DocumentPart(start));
                    }
                    if (TryReadLine(fileStreamReader, out string tempLine))
                    {
                        doc.PartBuffer.Last().Buffer.Add(tempLine);
                        doc.PartBuffer.Last().SizePart++;
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
                doc.IsLoadFromFile = true;
                doc.SetPath(path);
                doc.PathFrom = path;

                // Checking the existence of a file along a path
                if (!File.Exists(doc.GetPath()))
                {
                    throw new FileNotFoundException();
                }
                using StreamReader fileStreamReader = new StreamReader(doc.PathFrom);

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
                    if (lineIndex % PartSize == 0)
                    {
                        doc.PartBuffer.Add(new DocumentPart(start));
                    }
                    if (TryReadLine(fileStreamReader, out string tempLine))
                    {
                        doc.PartBuffer.Last().Buffer.Add(tempLine);
                        doc.PartBuffer.Last().SizePart++;
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

        public static bool TrySave(Document doc)
        {
            try
            {
                if (!doc.HasPath())
                {
                    return false;
                }

                if (!File.Exists(doc.PathFrom))
                {
                    using StreamWriter streamWriter1 = new StreamWriter(doc.GetPath());
                    foreach (string line in doc.PartBuffer.SelectMany(part => part.Buffer))
                        streamWriter1.WriteLine(line);
                    doc.IsSaved = true;
                    doc.IsLoadFromFile = true;
                    doc.PathFrom = doc.GetPath();
                    return true;
                }

                using StreamReader streamReader = new StreamReader(doc.PathFrom);
                using StreamWriter streamWriter = new StreamWriter(doc.GetPath());

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
                    foreach (DocumentPart docPart in doc.PartBuffer)
                    {
                        for (int lineIndex = 0; lineIndex < docPart.Buffer.Count; lineIndex++)
                        {
                            streamWriter.WriteLine(doc.PartBuffer[lineIndex - doc.PartOffset]);
                            if (lineIndex < docPart.SizePart)
                            {
                                streamReader.ReadLine();
                            }
                        }
                    }
                }

                {
                    string? line;
                    for (; (line = streamReader.ReadLine()) != null;)
                    {
                        streamWriter.WriteLine(line);
                    }
                }
                doc.IsSaved = true;
                doc.IsLoadFromFile = true;
                doc.PathFrom = doc.GetPath();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
