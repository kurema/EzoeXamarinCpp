using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using kurema.XamarinMarkdownView.Themes;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Xamarin.Forms;


namespace kurema.XamarinMarkdownView.Renderers
{
#nullable enable
    public class MarkdownRenderer : RendererBase
    {
        private Label CurrentLabel = new Label();
        private Layout<View> TopLayout = new StackLayout();
        private Stack<Tuple<Layout<View>, StyleSimple>> LayoutStack = new Stack<Tuple<Layout<View>, StyleSimple>>();
        public Layout<View>? TemporaryTargetLayout { get; set; } = null;
        private Layout<View> CurrentLayout => TemporaryTargetLayout ?? LayoutStack?.LastOrDefault()?.Item1 ?? TopLayout;

        public Uri? BasePath { get; set; }

        public Theme Theme { get; set; } = Theme.GetDefaultTheme();

        public Action<Uri> UriOpener { get; set; } = (uri) => OpenUri(uri);

        public List<TocEntry> Toc { get; } = new List<TocEntry>();

        /// <summary>
        /// Make sure to reset when done.
        /// </summary>
        public Uri? CurrentHyperlink { get; set; }
        public Theme.StyleId HyperlinkStyleId { get; set; } = Theme.StyleId.None;

        public MarkdownRenderer()
        {
            Clear();

            // Default block renderers
            ObjectRenderers.Add(new CodeBlockRenderer());
            ObjectRenderers.Add(new ListRenderer());
            ObjectRenderers.Add(new HeadingRenderer());
            ObjectRenderers.Add(new HtmlBlockRenderer());
            ObjectRenderers.Add(new ParagraphRenderer());
            ObjectRenderers.Add(new QuoteBlockRenderer());
            ObjectRenderers.Add(new ThematicBreakRenderer());

            // Default inline renderers
            ObjectRenderers.Add(new AutolinkInlineRenderer());
            ObjectRenderers.Add(new CodeInlineRenderer());
            ObjectRenderers.Add(new DelimiterInlineRenderer());
            ObjectRenderers.Add(new EmphasisInlineRenderer());
            ObjectRenderers.Add(new LineBreakInlineRenderer());
            ObjectRenderers.Add(new HtmlInlineRenderer());
            ObjectRenderers.Add(new HtmlEntityInlineRenderer());
            ObjectRenderers.Add(new LinkInlineRenderer());
            ObjectRenderers.Add(new LiteralInlineRenderer());
        }

        public override object Render(MarkdownObject markdownObject)
        {
            Clear();
            if (markdownObject is MarkdownDocument)
                Write(markdownObject);
            return GetView();
        }

        public void AppendInline(string? text, Theme.StyleId styleId)
        {
            if (text==null) return;

            var span = new Span();
            span.Text = text;
            var style = StyleSimple.Combine(LayoutStack.Select(a => a.Item2).ToArray());

            if (CurrentHyperlink != null)
            {
                var taper = new TapGestureRecognizer();
                taper.Tapped += (a, e) => UriOpener(CurrentHyperlink);
                span.GestureRecognizers.Add(taper);
                span.Style = StyleSimple.Combine(style, Theme.GetStyleFromStyleId(HyperlinkStyleId), Theme.GetStyleFromStyleId(styleId)).ToStyleSpan();
            }
            else
            {
                span.Style = StyleSimple.Combine(style, Theme.GetStyleFromStyleId(styleId)).ToStyleSpan();
            }
            AppendInline(span);
        }

        public void AppendHyperlink(string text, Theme.StyleId styleKey,System.Uri url)
        {
            var restoreUrl = CurrentHyperlink;
            var restoreStyle = HyperlinkStyleId;
            CurrentHyperlink = url;
            HyperlinkStyleId = Theme.StyleId.None;
            AppendInline(text, styleKey);
            CurrentHyperlink = restoreUrl;
            HyperlinkStyleId = restoreStyle;
        }

        private static async void OpenUri(Uri Url)
        {
            if (Url == null) return;
            if (!Url.IsFile)
            {
                //ConfigureAwait(true)で良いのかな？
                await Xamarin.Essentials.Browser.OpenAsync(Url, Xamarin.Essentials.BrowserLaunchMode.SystemPreferred).ConfigureAwait(true);
            }
        }

        public void AppendInline(Span span)
        {
            CurrentLabel = CurrentLabel ?? new Label();
            CurrentLabel.FormattedText.Spans.Add(span);
        }

        public void AppendBlock(View view)
        {
            CurrentLayout.Children.Add(view);
        }

        public void AppendFrame(Theme.StyleId styleKey)
        {
            var stack = new StackLayout();
            var theme = Theme.GetStyleFromStyleId(styleKey);
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(stack, theme));
            AppendBlock(
                new Frame()
                {
                    Style = theme.ToStyleFrame(),
                    Content = stack
                });
        }

        public void AppendStack(Theme.StyleId styleKey)
        {
            var stack = new StackLayout();
            var theme = Theme.GetStyleFromStyleId(styleKey);
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(stack, theme));
            AppendBlock(stack);
        }

        public void ApendQuote(Theme.StyleId styleBox, Theme.StyleId styleId)
        {
            StackLayout stack = new StackLayout();
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(stack, Theme.GetStyleFromStyleId(styleId)));

            AppendBlock(
                new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = {
                        new BoxView(){Style=Theme.GetStyleFromStyleId(styleBox).ToStyleBox()},
                        stack
                    }
                }
                );
        }

        public void AddTocEntry(Theme.StyleId styleId, string title,View view)
        {
            Toc.Add(new TocEntry() { StyleId = styleId, Title = title, View = view });
        }

        public void Clear()
        {
            TopLayout = new StackLayout();
            LayoutStack.Clear();
            Toc.Clear();
            CurrentLabel = new Label();
        }

        public void CloseLabel()
        {
            if (CurrentLabel == null) return;
            CurrentLayout.Children.Add(CurrentLabel);
            CurrentLabel = new Label();
        }

        public void AppendLine(string text, Theme.StyleId styleId)
        {
            CloseLabel();
            AppendInline(text, styleId);
            CloseLabel();
        }

        public void CloseLayout()
        {
            LayoutStack.Pop();
        }

        public View GetView()
        {
            CloseLabel();

            return TopLayout;
        }

        public void AppendLeafs(LeafBlock leaf, Theme.StyleId styleId, bool registerToc = false)
        {
            if (leaf == null) return;
            CloseLabel();
            var title = leaf.Lines.Lines.Aggregate("", (a, b) => a + "\n" + b);
            AppendInline(title, styleId);
            if (registerToc) AddTocEntry(styleId, title, CurrentLabel);
            CloseLabel();
        }

        public static Uri? GetAbsoluteUri(Uri? basePath, string path)
        {
            if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                return new Uri(path);
            }
            else if (Uri.IsWellFormedUriString(path, UriKind.Relative) && basePath != null)
            {
                return new Uri(basePath, path);
            }
            else
            {
                return null;
            }
        }
    }

    public struct TocEntry : IEquatable<TocEntry>
    {
        public View View { get; set; }
        public string Title { get; set; }
        public Theme.StyleId StyleId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TocEntry entry && Equals(entry);
        }

        public bool Equals(TocEntry other)
        {
            return EqualityComparer<View>.Default.Equals(View, other.View) &&
                   Title == other.Title &&
                   StyleId == other.StyleId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(View, Title, StyleId);
        }

        public static bool operator ==(TocEntry left, TocEntry right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TocEntry left, TocEntry right)
        {
            return !(left == right);
        }
    }

    public abstract class XamarinFormsObjectRenderer<TObject> : MarkdownObjectRenderer<MarkdownRenderer, TObject> where TObject : MarkdownObject
    {
    }
#nullable restore
}
