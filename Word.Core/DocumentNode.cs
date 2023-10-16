﻿namespace Word.Core
{
    public enum DocumentNodeType 
    {
        Text = 0,
        List = 1,
        ListItem = 2,
        Table = 3,
        TableCell = 4,
        TableRow = 5,
    }

    public class DocumentNode
    {
        public DocumentNodeType Type;
        public string Content = string.Empty;
        public List<DocumentNode> ChildNodes = new List<DocumentNode>();
    }
}