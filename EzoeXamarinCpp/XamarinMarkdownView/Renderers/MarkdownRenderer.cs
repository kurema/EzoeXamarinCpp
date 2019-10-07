﻿using System;
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
        private Layout<View> CurrentLayout;

        private Theme theme = Theme.GetDefaultTheme();

        public Theme Theme { get => theme; set => theme = value; }

        public MarkdownRenderer()
        {
            CurrentLayout = TopLayout;
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
            span.Style = Theme.GetStyleFromStyleId(styleId);
            AppendInline(span);
        }

        public void AppendHyperLink(string text, Theme.StyleId styleKey,System.Uri url)
        {
            var span = new Span();
            span.Text = text;
            span.Style = Theme.GetStyleFromStyleId(styleKey);
            {
                var taper = new TapGestureRecognizer();
                taper.Tapped += (a, e) => OpenBrowser(url.OriginalString);
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
            CurrentLayout.Children.Add(view);
        }

        public void AppendStack(Theme.StyleId styleKey)
        {
            AppendBlock(new StackLayout() { Style = Theme.GetStyleFromStyleId(styleKey) });
        }

        public void Clear()
        {
            TopLayout = new StackLayout();
            CurrentLayout = TopLayout;
            CurrentLabel = new Label();
        }

        public void CloseLabel()
        {
            if (CurrentLabel == null) return;
            CurrentLayout.Children.Add(CurrentLabel);
            CurrentLabel = new Label();
        }

        public void CloseLayout()
        {
            CurrentLayout = TopLayout;
        }

        public View GetView()
        {
            CloseLabel();

            return TopLayout;
        }
    }

    public abstract class ObjectRenderer<TObject> : MarkdownObjectRenderer<MarkdownRenderer, TObject> where TObject : MarkdownObject
    {
    }
#nullable restore
}
