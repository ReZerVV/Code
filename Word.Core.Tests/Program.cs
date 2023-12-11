using System.Text;
using Word.Core;
using Word.Core.Syntax.Lexers;
using Word.Core.Syntax.Markers;
using Word.Core.Syntax.Styles;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Test_LoadFilePart();
        Console.Write("\n\n");
        Test_LexerTokenize();
        Console.Write("\n\n");
        Test_MarkupJavaScript();
        Console.Write("\n\n");
        Test_MarkupPython();
        Console.Write("\n\n");
        Test_MarkupCOrCpp();
        Console.Write("\n\n");
        Test_MarkupSQL();
        Console.Write("\n\n");
        Test_MarkupHTML();
        Console.Write("\n\n");
        Test_Editing();
        Console.Write("\n\n");
        Test_DynamicLoafFilePart();
    }

    private static void LogErr(string message)
    {
        Console.Write("\t");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.Write("err:");
        Console.ResetColor();
        Console.Write($"\t{message}");
        Console.Write('\n');
    }

    private static void LogInfo(string message)
    {
        Console.Write("\t");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Write("info:");
        Console.ResetColor();
        Console.Write($"\t{message}");
        Console.Write('\n');
    }

    private static void CreateTestFile(string path, int lineCount = 100)
    {
        using StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8);
        for (int lineIndex = 0; lineIndex < lineCount; lineIndex++) 
        {
            streamWriter.WriteLine($"{lineIndex + 1}:\tLorem ipsum dolor sit amet, consectetur adipiscing elit.");
        }
    }

    private static void RemoveTestFile(string path)
    {
        File.Delete(path);
    }

    public static void Test_LoadFilePart()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_LoadFilePart)}");

        string path = $"./{nameof(Test_LoadFilePart)}.txt";
        
        CreateTestFile(path);
        Document doc = new Document();
        doc.SetPath(path);
        
        int docLineOffset = 0;
        int partSize = 23;
        int lineCount = doc.GetTotalLineCount();
        
        if (doc.TryLoadFilePart(docLineOffset, partSize))
        {
            LogInfo("File part uploaded successfully");
        }
        else
        {
            LogErr("An error occurred while downloading the file");
        }
        for (int lineIndex = 0; lineIndex < lineCount; lineIndex++)
        {
            if (lineIndex >= docLineOffset + partSize)
            {
                docLineOffset += partSize;
                if (doc.TryLoadFilePart(docLineOffset, partSize))
                {
                    LogInfo("File part uploaded successfully");
                }
                else
                {
                    LogErr("An error occurred while downloading the file");
                    break;
                }
            }
            Console.WriteLine(doc.Buffer[lineIndex - docLineOffset]);
        }

        RemoveTestFile(path);
    }

    public static void Test_DynamicLoafFilePart()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_DynamicLoafFilePart)}");

        string path = $"./{nameof(Test_DynamicLoafFilePart)}.txt";

        CreateTestFile(path);

        Document doc = new Document();
        doc.SetPath(path);
        DocumentCursor cursor = new DocumentCursor(doc);

        int lineCount = doc.GetTotalLineCount();
        for (; cursor.TryMoveDown(cursor.PartSize);)
        {
            foreach (string line in doc.Buffer)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("\n");
        }

        RemoveTestFile(path);
    }

    public static void Test_LexerTokenize()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_LexerTokenize)}");

        string code = "int main() {" +
            "   std::cout << \"Hello world\" << std::endl;" +
            "   return 0;" +
            "}";

        LogInfo("Tokenize");

        foreach (Token token in DefaultLexer.Tokenize(code))
        {
            Console.WriteLine($"(Type:{token.Type})\t\t\t\"{token.Value}\"");
        }
    }

    public static void Test_MarkupJavaScript()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_MarkupJavaScript)}");

        List<string> code = new List<string>() 
        {
            "// the hello world program",
            "console.log('Hello World');",
        };

        LogInfo("Markup");
        IMarker marker = new JavaScriptMarker();
        foreach (List<RichText> richTextList in marker.Markup(code))
        {
            foreach (RichText richText in richTextList)
            {
                Console.Write($"\u001b[38;2;{richText.ColorOptions.R};{richText.ColorOptions.G};{richText.ColorOptions.B}m{richText.Value}");
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }

    public static void Test_MarkupPython()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_MarkupPython)}");

        List<string> code = new List<string>()
        {
            "# declaring the var",
            "Number = 100",
            "",
            "# display",
            "print(\"Before declare: \", Number)",
            "",
            "# re-declare the var",
            "Number = 120.3",
            "",
            "print(\"After re-declare:\", Number)",
        };

        LogInfo("Markup");
        IMarker marker = new PythonMarker();
        foreach (List<RichText> richTextList in marker.Markup(code))
        {
            foreach (RichText richText in richTextList)
            {
                Console.Write($"\u001b[38;2;{richText.ColorOptions.R};{richText.ColorOptions.G};{richText.ColorOptions.B}m{richText.Value}");
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }

    public static void Test_MarkupCOrCpp()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_MarkupCOrCpp)}");

        List<string> code = new List<string>()
        {
            "// C++ program to illustrate the use",
            "// of identifiers",
            "",
            "#include <iostream>",
            "using namespace std;",
            "",
            "// Driver Code",
            "int main()",
            "{",
            "\t// Use of Underscore (_) symbol",
            "\t// in variable declaration",
            "\tint geeks_for_geeks = 1;",
            "\tcout << \"Identifier result is: \"",
            "\t\t<< geeks_for_geeks;",
            "\treturn 0;",
            "}",
        };

        LogInfo("Markup");
        IMarker marker = new COrCppMarker();
        foreach (List<RichText> richTextList in marker.Markup(code))
        {
            foreach (RichText richText in richTextList)
            {
                Console.Write($"\u001b[38;2;{richText.ColorOptions.R};{richText.ColorOptions.G};{richText.ColorOptions.B}m{richText.Value}");
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }

    public static void Test_MarkupSQL()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_MarkupSQL)}");

        List<string> code = new List<string>()
        {
            "--asdasdada",
            "SELECT * FROM Products",
            "WHERE Price BETWEEN 10 AND 20;"
        };

        LogInfo("Markup");
        IMarker marker = new SqlMarker();
        foreach (List<RichText> richTextList in marker.Markup(code))
        {
            foreach (RichText richText in richTextList)
            {
                Console.Write($"\u001b[38;2;{richText.ColorOptions.R};{richText.ColorOptions.G};{richText.ColorOptions.B}m{richText.Value}");
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }

    public static void Test_MarkupHTML()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_MarkupHTML)}");

        List<string> code = new List<string>()
        {
            "<!DOCTYPE html>",
            "<html>",
            "<head>",
            "<title>Page Title</title>",
            "</head>",
            "<body>",
            "",
            "<h1>This is a Heading</h1>",
            "<p>This is a paragraph.</p>",
            "",
            "</body>",
            "</html>",
        };

        LogInfo("Markup");
        IMarker marker = new HtmlMarker();
        foreach (List<RichText> richTextList in marker.Markup(code))
        {
            foreach (RichText richText in richTextList)
            {
                Console.Write($"\u001b[38;2;{richText.ColorOptions.R};{richText.ColorOptions.G};{richText.ColorOptions.B}m{richText.Value}");
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }

    public static void Test_Editing()
    {
        Console.WriteLine($"🧪 Test: {nameof(Test_Editing)}");

        Document doc = new Document();
        DocumentCursor cursor = new DocumentCursor(doc);

        doc.InsertTab(cursor);
        doc.InsertText("Hello world", cursor);
        doc.InsertNewLine(cursor);
        doc.InsertNewLine(cursor);
        doc.InsertTab(cursor);
        doc.InsertText("Hello sworld", cursor);

        foreach (string line in doc.Buffer)
        {
            Console.WriteLine(line);
        }
    }
}
