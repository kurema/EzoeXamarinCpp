using System;
using System.Collections.Generic;
using System.Text;

using Markdig.Syntax;
using Xamarin.Forms;
using Markdig.Syntax.Inlines;

using kurema.XamarinMarkdownView.Themes;

namespace kurema.XamarinMarkdownView.Renderers
{
#nullable enable
    public class AutolinkInlineRenderer : XamarinFormsObjectRenderer<AutolinkInline>
    {
        protected override void Write(MarkdownRenderer renderer, AutolinkInline obj)
        {
            if (obj == null) return;
            var address = (obj.IsEmail ? "mailto:" : "") + obj.Url;
            renderer?.AppendHyperlink(obj.Url, Theme.StyleId.Hyperlink, new Uri(address));
        }
    }

    public class CodeInlineRenderer : XamarinFormsObjectRenderer<CodeInline>
    {
        protected override void Write(MarkdownRenderer renderer, CodeInline obj)
        {
            if (obj == null) return;
            renderer?.AppendInline(obj.Content, Theme.StyleId.Code);
        }
    }

    public class DelimiterInlineRenderer : XamarinFormsObjectRenderer<DelimiterInline>
    {
        protected override void Write(MarkdownRenderer renderer, DelimiterInline obj)
        {
            //なにこれ？
            if (obj == null) return;
            renderer?.AppendInline(obj.ToLiteral(), Theme.StyleId.None);
        }
    }

    public class EmphasisInlineRenderer : XamarinFormsObjectRenderer<EmphasisInline>
    {
        protected override void Write(MarkdownRenderer renderer, EmphasisInline obj)
        {
            if (obj?.Span == null) return;
            if (renderer == null) return;
            renderer?.WriteChildrenWithStyle(obj, GetStyleIdByDelim(obj));
        }

        public static Theme.StyleId GetStyleIdByDelim(EmphasisInline span)
        {
            switch (span?.DelimiterChar)
            {
                case '*' when span.DelimiterCount == 2: // bold
                case '_' when span.DelimiterCount == 2: // bold
                    return Theme.StyleId.EmphasisBold;
                case '*': // italic
                case '_': // italic
                    return Theme.StyleId.EmphasisItalic;
                case '~': // strike through
                    return Theme.StyleId.StrikeThrough;
                case '^': // superscript, subscript
                    if (span.DelimiterCount == 2)
                        return Theme.StyleId.Superscript;
                    else
                        return Theme.StyleId.Subscript;
                case '+': // underline
                    return Theme.StyleId.Inserted;
                case '=': // Marked
                    return Theme.StyleId.Marked;
                default:
                    return Theme.StyleId.None;
            }
        }
    }

    public class HtmlEntityInlineRenderer : XamarinFormsObjectRenderer<HtmlEntityInline>
    {
        protected override void Write(MarkdownRenderer renderer, HtmlEntityInline obj)
        {
            renderer?.AppendInline(obj?.Transcoded.ToString(), Theme.StyleId.None);
        }
    }

    //public class HtmlInlineRenderer : XamarinFormsObjectRenderer<HtmlInline>
    //{
    //    protected override void Write(MarkdownRenderer renderer, HtmlInline obj)
    //    {
    //        renderer?.AppendInline(obj?.Tag, Theme.StyleId.None);
    //    }
    //}

    public class LineBreakInlineRenderer : XamarinFormsObjectRenderer<LineBreakInline>
    {
        protected override void Write(MarkdownRenderer renderer, LineBreakInline obj)
        {
            if (obj == null) return;
            if (obj.IsHard)
            {
                renderer?.AppendInline("\n", Theme.StyleId.None);
                //renderer?.CloseLabel();
            }
        }
    }

    public class LinkInlineRenderer : XamarinFormsObjectRenderer<LinkInline>
    {
        protected override void Write(MarkdownRenderer renderer, LinkInline obj)
        {
            if (obj == null) return;
            if (renderer == null) return;

            var url = obj.GetDynamicUrl?.Invoke() ?? obj.Url;
            Uri? uri = MarkdownRenderer.GetAbsoluteUri(renderer.BasePath, url);

            if (uri == null)
            {
                renderer.WriteChildren(obj);
                return;
            }

            var title = string.IsNullOrEmpty(obj.Title) ? obj.Url : obj.Title;

            if (obj.IsImage)
            {
                renderer.AppendBlock(new Image()
                {
                    Source = ImageSource.FromUri(uri),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                });
            }
            else
            {
                var restore = renderer.CurrentHyperlink;
                renderer.CurrentHyperlink = uri;
                renderer.HyperlinkStyleId = Theme.StyleId.Hyperlink;
                renderer.WriteChildren(obj);
                renderer.CurrentHyperlink = restore;
            }
        }
    }

    public class LiteralInlineRenderer : XamarinFormsObjectRenderer<LiteralInline>
    {
        protected override void Write(MarkdownRenderer renderer, LiteralInline obj)
        {
            //これもなんだろう？
            renderer?.AppendInline(obj?.Content.ToString(), Theme.StyleId.None);
        }
    }
}
