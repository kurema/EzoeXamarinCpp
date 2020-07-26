using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace kurema.XamarinCodeView
{
    public static class LineNumberProvider
    {
        public static Grid GetLineNumber(Style lineNumberLabelStyle, params Label[] labels)
        {
            void AddLine(int lineCount, int rowCount, Grid grid, View label1)
            {
                grid.Children.Add(new Label()
                {
                    Text = lineCount.ToString(),
                    Style = lineNumberLabelStyle
                }, 0, rowCount);
                grid.Children.Add(label1, 1, rowCount);
            }

            Label CopyLabelStyle(Label original)
            {
                return new Label()
                {
                    Style=original.Style,
                    FontFamily=original.FontFamily,
                    FontAttributes=original.FontAttributes,
                    FontSize=original.FontSize,
                    BackgroundColor=original.BackgroundColor,
                    TextColor=original.TextColor,
                    TextDecorations=original.TextDecorations,
                    LineBreakMode=original.LineBreakMode,
                    FormattedText=new FormattedString()
                };
            }

            Span CopySpanStyle(Span original)
            {
                return new Span()
                {
                    Style = original.Style,
                    FontFamily = original.FontFamily,
                    FontAttributes = original.FontAttributes,
                    FontSize = original.FontSize,
                    BackgroundColor = original.BackgroundColor,
                    TextColor = original.TextColor,
                    TextDecorations = original.TextDecorations,
                };
            }

            if (labels == null) return new Grid();

            var result = new Grid();

            int rowCount = 0;
            foreach (var label in labels)
            {
                if (label == null) continue;

                if (label.FormattedText == null)
                {
                    label.FormattedText = new FormattedString();
                    label.FormattedText.Spans.Add(new Span() { Text = label.Text });
                }
                var CurrentLabel = CopyLabelStyle(label);
                foreach (var item in label.FormattedText.Spans)
                {
                    var texts = item.Text.Split('\n');
                    if (texts.Length > 0)
                    {
                        var span = CopySpanStyle(item);
                        span.Text = texts[0];
                        CurrentLabel.FormattedText.Spans.Add(span);
                    }
                    for(int i = 1; i < texts.Length; i++)
                    {
                        AddLine(rowCount + 1, rowCount, result, CurrentLabel);
                        CurrentLabel = CopyLabelStyle(label);
                        rowCount++;

                        var span = CopySpanStyle(item);
                        span.Text = texts[i];
                        CurrentLabel.FormattedText.Spans.Add(span);
                    }
                }
                {
                    AddLine(rowCount + 1, rowCount, result, CurrentLabel);
                    rowCount++;
                }
            }

            return result;
        }
    }
}
