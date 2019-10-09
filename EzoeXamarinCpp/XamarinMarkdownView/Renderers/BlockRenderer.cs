using System;
using System.Collections.Generic;
using System.Text;

using Markdig.Syntax;
using Xamarin.Forms;

namespace kurema.XamarinMarkdownView.Renderers
{
    public class ParagraphRenderer : XamarinFormsObjectRenderer<ParagraphBlock>
    {
        protected override void Write(MarkdownRenderer renderer, ParagraphBlock obj)
        {
            if (renderer == null) return;
            if (obj == null) return;
            renderer.AppendStack(Themes.Theme.StyleId.Paragraph);
            foreach(var item in obj.Lines.Lines)
            {
                renderer.AppendInline(item.Slice.ToString(), Themes.Theme.StyleId.None);
            }
            renderer.CloseLayout();
        }
    }

    public class ListRenderer : XamarinFormsObjectRenderer<ListBlock>
    {
        protected override void Write(MarkdownRenderer renderer, ListBlock obj)
        {
            int bulletCount = 1;
            if (obj == null) return;
            if (renderer == null) return;
            if (obj.IsOrdered)
            {
                if(obj.BulletType != '1')
                {
                    //throw new NotImplementedException();
                }
                if(obj.OrderedStart!=null && obj.OrderedStart != "1")
                {
                    if (!int.TryParse(obj.OrderedStart, out bulletCount)) bulletCount = 1;
                }
            }

            var grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(){Width=GridLength.Auto},
                    new ColumnDefinition(){Width=GridLength.Star},
                }
            };

            int rowCount = 0;
            foreach (ListItemBlock item in obj)
            {
                //"・"はBoxViewの方が良いかな？
                grid.Children.Add(
                    new Label()
                    {
                        Text = obj.IsOrdered ? bulletCount + "." : "・",
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        Style = renderer.Theme.GetStyleFromStyleId(Themes.Theme.StyleId.ListItem).ToStyleLabel()
                    }, 0, rowCount
                    );
                var stack = new StackLayout();
                grid.Children.Add(stack, 1, rowCount);
                renderer.TemporaryTargetLayout = stack;
                renderer.WriteChildren(obj);
                renderer.TemporaryTargetLayout = null;
            }

            renderer.AppendBlock(grid);
        }
    }

    public class QuoteBlockRenderer : XamarinFormsObjectRenderer<QuoteBlock>
    {
        protected override void Write(MarkdownRenderer renderer, QuoteBlock obj)
        {
            if (renderer == null) return;
            renderer.AppendFrame(Themes.Theme.StyleId.QuoteBlock);
            renderer.WriteChildren(obj);
            renderer.CloseLayout();
        }
    }

    public class HtmlBlockRenderer : XamarinFormsObjectRenderer<HtmlBlock>
    {
        protected override void Write(MarkdownRenderer renderer, HtmlBlock obj)
        {
            renderer?.AppendLeafs(obj,Themes.Theme.StyleId.HtmlBlock);
        }
    }


}
