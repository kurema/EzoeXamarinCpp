using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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
        private List<View> Layouts = new List<View>();
        private Label? CurrentLabel = null;

        public MarkdownRenderer()
        {
        }

        public override object Render(MarkdownObject markdownObject)
        {
            Clear();
            if (markdownObject is MarkdownDocument)
                Write(markdownObject);
            return GetView();
        }

        public void AppendInline(string text, string styleId)
        {
            var span = new Span();
            span.Text = text;
            span.StyleId = styleId;
            AppendInline(span);
        }

        public void AppendHyperLink(string text,string styleId,string Url)
        {
            var span = new Span();
            span.Text = text;
            span.StyleId = styleId;
            {
                var taper = new TapGestureRecognizer();
                taper.Tapped += (a, e) => OpenBrowser(Url);
                span.GestureRecognizers.Add(taper);
            }
            AppendInline(span);
        }

        private static async void OpenBrowser(string Url) => await Xamarin.Essentials.Browser.OpenAsync(Url, Xamarin.Essentials.BrowserLaunchMode.SystemPreferred);

        public void AppendInline(Span span)
        {
            CurrentLabel = CurrentLabel ?? new Label();
            CurrentLabel.FormattedText.Spans.Add(span);
        }

        public void AppendBlock(View view)
        {
            Layouts.Add(view);
        }

        public void Clear()
        {
            Layouts = new List<View>();
        }

        public void CloseLabel()
        {
            if (CurrentLabel == null) return;
            Layouts.Add(CurrentLabel);
            CurrentLabel = null;
        }

        public View GetView()
        {
            CloseLabel();

            var layout = new StackLayout();
            foreach (var item in Layouts)
                layout.Children.Add(item);
            return layout;
        }
    }

    public abstract class ObjectRenderer<TObject> : MarkdownObjectRenderer<MarkdownRenderer, TObject> where TObject : MarkdownObject
    {
    }
#nullable restore
}
