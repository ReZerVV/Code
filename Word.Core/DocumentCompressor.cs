using System.Text;

namespace Word.Core
{
    // Helper for document comperss lines
    public class DocumentCompressor
    {
        public static string СompressingLinesIntoLine(List<string> lines)
        {
            StringBuilder compressTextBuilder = new StringBuilder();
            foreach (string line in lines)
            {
                compressTextBuilder.Append(line);
            }
            return compressTextBuilder.ToString();
        }

        public static string СompressingLinesIntoLineAndFixingNewLine(List<string> lines, string newLineSign = "\n")
        {
            StringBuilder compressTextBuilder = new StringBuilder();
            foreach (string line in lines)
            {
                compressTextBuilder.Append(line + newLineSign);
            }
            return compressTextBuilder.ToString();
        }
    }
}
