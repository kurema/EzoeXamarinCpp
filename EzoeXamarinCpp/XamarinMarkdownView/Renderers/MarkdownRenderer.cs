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

        public Theme Theme { get; set; } = Theme.GetDefaultTheme();

        public MarkdownRenderer()
        {
            Clear();
        }

        public override object Render(MarkdownObject markdownObject)
        {
            Clear();
            if (markdownObject is MarkdownDocument)
                Write(markdownObject);
            return GetView();
        }

        public void AppendInline(string text, Theme.StyleId styleId)
        {
            var span = new Span();
            span.Text = text;
            span.Style = Theme.GetStyleFromStyleId(styleId).ToStyleSpan();
            AppendInline(span);
        }

        public void AppendHyperLink(string text, Theme.StyleId styleKey,System.Uri url)
        {
            var span = new Span();
            span.Text = text;
            span.Style = Theme.GetStyleFromStyleId(styleKey).ToStyleSpan();
            {
                var taper = new TapGestureRecognizer();
                taper.Tapped += (a, e) => OpenUri(url);
                span.GestureRecognizers.Add(taper);
            }
            AppendInline(span);
        }

        private static async void OpenUri(Uri Url)
        {
            await Xamarin.Essentials.Browser.OpenAsync(Url, Xamarin.Essentials.BrowserLaunchMode.SystemPreferred);
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

        public void ApendQuote(Theme.StyleId styleBox,Theme.StyleId styleContent)
        {
            StackLayout stack = new StackLayout();
            LayoutStack.Push(new Tuple<Layout<View>, StyleSimple>(stack, Theme.GetStyleFromStyleId(styleContent)));

            AppendBlock(
                new StackLayout
                {
                    Orientation=StackOrientation.Horizontal,
                    Children = {
                        new BoxView(){Style=Theme.GetStyleFromStyleId(styleBox).ToStyleBox()},
                        stack
                    }
                }
                );
        }

        public void Clear()
        {
            TopLayout = new StackLayout();
            LayoutStack.Clear();
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

        public void AppendLeafs(LeafBlock leaf, Theme.StyleId styleId)
        {
            if (leaf == null) return;
            foreach(var item in leaf.Lines.Lines)
            {
                AppendLine(item.Slice.ToString(),styleId);
            }
        }
    }

    public abstract class XamarinFormsObjectRenderer<TObject> : MarkdownObjectRenderer<MarkdownRenderer, TObject> where TObject : MarkdownObject
    {
    }
#nullable restore
}
