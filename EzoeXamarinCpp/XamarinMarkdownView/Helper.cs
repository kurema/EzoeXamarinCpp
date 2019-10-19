using System;
using System.Collections.Generic;
using System.Linq;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace kurema.XamarinMarkdownView
{
    public static class Helper
    {
        public static string LeafToString(LeafBlock leaf)
        {
            if (leaf?.Lines.Lines == null) return "";
            return String.Join('\n', leaf.Lines.Lines.Select(a => a.ToString()));
        }
    }
}
