using System;
using System.Collections.Generic;
using System.Linq;
using kurema.XamarinMarkdownView.Renderers.SectionProviders;
using kurema.XamarinMarkdownView.Themes;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Xamarin.Forms;
using kurema.XamarinMarkdownView.Renderers;

namespace kurema.XamarinMarkdownView
{
#nullable enable
    public class MarkdownRenderer : RendererBase
    {
        private Label? CurrentLabel = new Label();
        private Layout<View> TopLayout = new StackLayout();
        private Stack<Tuple<Layout<View>, StyleSimple>> LayoutStack = new Stack<Tuple<Layout<View>, StyleSimple>>();
        public Layout<View>? TemporaryTargetLayout { get; set; } = null;
        private Layout<View> CurrentLayout => TemporaryTargetLayout ?? LayoutStack?.LastOrDefault()?.Item1 ?? TopLayout;

        public Uri? BasePath { get; set; }

        public Theme Theme { get; set; } = Theme.GetDefaultTheme(ThemeColors.Dark);

        public Action<Uri> UriOpener { get; set; } = (uri) => OpenUri(uri);

        public List<TocEntry> Toc { get; } = new List<TocEntry>();
        private IHeadingProvider headingProvider = new SectionProviderEmpty();

        /// <summary>
        /// Make sure to reset when done.
        /// </summary>
        public Uri? CurrentHyperlink { get; set; }
        public Theme.StyleId HyperlinkStyleId { get; set; } = Theme.StyleId.None;
        public IHeadingProvider HeadingProvider { get => headingProvider = headingProvider ?? new SectionProviderEmpty(); set => headingProvider = value; }

        public List<int> CurrentSection { get; private set; } = new List<int>();

        public MarkdownRenderer()
        {
            Clear();

            // Default block renderers
            ObjectRenderers.Add(new CodeBlockRenderer());
            ObjectRenderers.Add(new ListRenderer());
            ObjectRenderers.Add(new HeadingRenderer());
            //ObjectRenderers.Add(new HtmlBlockRenderer());
            ObjectRenderers.Add(new ParagraphRenderer());
            ObjectRenderers.Add(new QuoteBlockRenderer());
            ObjectRenderers.Add(new ThematicBreakRenderer());

            // Default inline renderers
            ObjectRenderers.Add(new AutolinkInlineRenderer());
            ObjectRenderers.Add(new CodeInlineRenderer());
            ObjectRenderers.Add(new DelimiterInlineRenderer());
            ObjectRenderers.Add(new EmphasisInlineRenderer());
            ObjectRenderers.Add(new LineBreakInlineRenderer());
            //ObjectRenderers.Add(new HtmlInlineRenderer());
            ObjectRenderers.Add(new HtmlEntityInlineRenderer());
            ObjectRenderers.Add(new LinkInlineRenderer());
            ObjectRenderers.Add(new LiteralInlineRenderer());
        }

        public override object Render(MarkdownObject markdownObject)
        {
            return Render(markdownObject, false);

        }

        public object Render(MarkdownObject markdownObject, bool isScrollView)
        {
            Clear();
            if (markdownObject is MarkdownDocument)
                Write(markdownObject);
            return GetView(isScrollView);
        }

        public void AppendInline(string? text, Theme.StyleId styleId)
        {
            AppendInline(text, Theme.GetStyleFromStyleId(styleId));
        }
        public void AppendInline(string? text, StyleSimple style)
        {
            if (text==null) return;

            var span = new Span();
            span.Text = text;
            var styleBase = StyleSimple.Combine(LayoutStack.Select(a => a.Item2).ToArray());

            if (CurrentHyperlink != null)
            {
                var taper = new TapGestureRecognizer();
                var link = CurrentHyperlink;
                taper.Tapped += (a, e) => UriOpener(link);
                span.GestureRecognizers.Add(taper);
                span.Style = StyleSimple.CombineLast(styleBase, Theme.GetStyleFromStyleId(HyperlinkStyleId), Theme.GetStyleFromStyleId(styleId)).ToStyleSpan();
                var styleTest = Theme.GetStyleFromStyleId(HyperlinkStyleId);
            }
            else
            {
                span.Style = StyleSimple.CombineLast(styleBase, style).ToStyleSpan();
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

        public void AppendHorizontalLine(Theme.StyleId styleKey)
        {
            var style = Theme.GetStyleFromStyleId(styleKey);
            if (style.BorderSize == 0) return;
            AppendBlock(new BoxView()
            {
                Style = style.ToStyleBox(),
                WidthRequest = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });
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
            CurrentLabel.FormattedText = CurrentLabel.FormattedText ?? new FormattedString();
            CurrentLabel.FormattedText.Spans.Add(span);
        }

        public void AppendBlock(View view)
        {
            CurrentLayout.Children.Add(view);
        }

        public void AppendFrame(Theme.StyleId styleKey)
        {
            CloseLabel();
            CurrentLabel = null;
            var stack = new StackLayout();
            var theme = Theme.GetStyleFromStyleId(styleKey);
            AppendBlock(
                new Frame()
                {
                    Style = theme.ToStyleFrame(),
                    Content = stack
                });
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(stack, theme));
        }

        public void AppendStack(Theme.StyleId styleKey)
        {
            var stack = new StackLayout();
            var theme = Theme.GetStyleFromStyleId(styleKey);
            AppendBlock(stack);
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(stack, theme));
        }

        public void ApendQuote(Theme.StyleId styleBox, Theme.StyleId styleId)
        {
            StackLayout stack = new StackLayout();

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
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(stack, Theme.GetStyleFromStyleId(styleId)));
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
            CurrentLabel = null;
        }

        public void CloseLabel()
        {
            if (CurrentLabel == null) return;
            CurrentLayout.Children.Add(CurrentLabel);
            CurrentLabel = null;
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

        public View GetView(bool isScrollView=false)
        {
            CloseLabel();

            var style = Theme.GetStyleFromStyleId(Theme.StyleId.Document);
            TopLayout.Margin = style.Margin ?? TopLayout.Margin;

            if (isScrollView)
            {
                return new ScrollView()
                {
                    Content = TopLayout,
                    BackgroundColor = style.BackgroundColor ?? Color.Transparent
                };
            }
            else
            {
                TopLayout.BackgroundColor = style.BackgroundColor ?? TopLayout.BackgroundColor;
                return TopLayout;
            }
        }

        public void AppendLeafRawLines(LeafBlock leaf, Theme.StyleId styleId, bool registerToc = false
            )
        {
            if (leaf?.Lines.Lines == null) return;
            CloseLabel();
            var title = String.Join('\n', leaf.Lines.Lines.Select(a => a.ToString()));
            AppendInline(title, styleId);
            CurrentLabel = CurrentLabel ?? new Label();
            if (registerToc) AddTocEntry(styleId, title, CurrentLabel);
            CloseLabel();
        }

        public void WriteChildrenWithStyle(ContainerInline container, Theme.StyleId styleId)
        {
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(new StackLayout(), Theme.GetStyleFromStyleId(styleId)));
            WriteChildren(container);
            LayoutStack.Pop();
        }

        public void AppendLeafInline(LeafBlock leafBlock, Theme.StyleId styleId, bool registerToc = false
            , string sectionHeader = "", StyleSimple? sectionHeaderStyle = null)
        {
            if (leafBlock == null) return;
            CloseLabel();

            var title = "";
            var style = Theme.GetStyleFromStyleId(styleId);
            if (!String.IsNullOrEmpty(sectionHeader))
            {
                AppendInline(sectionHeader, sectionHeaderStyle ?? style);
                title += sectionHeader;
            }

            var inline = (Inline)leafBlock.Inline;
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(new StackLayout(), style));
            while (inline != null)
            {
                Write(inline);
                inline = inline.NextSibling;
            }
            LayoutStack.Pop();

            CurrentLabel = CurrentLabel ?? new Label();
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
