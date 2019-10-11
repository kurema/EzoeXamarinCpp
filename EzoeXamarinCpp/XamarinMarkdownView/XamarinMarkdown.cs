using System;
using System.Collections.Generic;
using System.Text;
using Markdig;

namespace kurema.XamarinMarkdownView
{
    public static class XamarinMarkdown
    {
        public static Xamarin.Forms.View ToXamarinForms(string markdown, MarkdownPipeline pipeline = null, bool isScrollView = false)
        {
            if (markdown == null)
                throw new ArgumentNullException(nameof(markdown));
            if (pipeline == null)
                pipeline = new MarkdownPipelineBuilder().Build();

            var renderer = new Renderers.MarkdownRenderer();
            pipeline.Setup(renderer);

            var document = Markdig.Markdown.Parse(markdown, pipeline);
            return renderer.Render(document, isScrollView) as Xamarin.Forms.View;
        }
    }
}
