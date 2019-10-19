using System;
using System.Collections.Generic;
using System.Text;

using Markdig.Syntax;
using Xamarin.Forms;

using Markdig.Extensions.Mathematics;
using Markdig.Extensions.Tables;

namespace kurema.XamarinMarkdownView.Renderers
{
    public class MathBlockRenderer : XamarinFormsObjectRenderer<MathBlock>
    {
        protected override void Write(MarkdownRenderer renderer, MathBlock obj)
        {
            if (renderer == null) return;

            renderer.AppendFrame(Themes.Theme.StyleId.MathBlock);
            renderer.AppendLeafRawLines(obj, Themes.Theme.StyleId.None);
            renderer.CloseLayout();
        }
    }

    
    public class MathBlockRendererMathJax : XamarinFormsObjectRenderer<MathBlock>
    {
        public string MathJaxPath { get; set; }
        = "https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/latest.js?config=TeX-MML-AM_CHTML";

        protected override void Write(MarkdownRenderer renderer, MathBlock obj)
        {
            if (renderer == null) return;

            var webview = new WebView();
            var source = new HtmlWebViewSource();
            source.Html = @"<html><head><script src=""" + MathJaxPath + @""" type=""text/javascript""></script></head><body>" +
                "\\[" + Helper.LeafToString(obj) + "\\]" + @"</body></html>";
            webview.Source = source;
            webview.VerticalOptions = LayoutOptions.StartAndExpand;

            renderer.AppendBlock(new Frame()
            {
                Style = renderer.Theme.GetStyleFromStyleId(Themes.Theme.StyleId.MathBlock).ToStyleFrame(),
                Content = webview
            });
        }
    }
}
