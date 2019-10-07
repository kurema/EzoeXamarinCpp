using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace kurema.XamarinMarkdownView.Themes
{
#nullable enable
    public class StyleSimple
    {
        public StyleSimple(FontAttributes? fontAttributes=null, double? fontSize = null, Color? foregroundColor = null, Color? backgroundColor = null, string? fontFamily = null)
        {
            FontAttributes = fontAttributes;
            FontSize = fontSize;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            FontFamily = fontFamily;
        }

        public FontAttributes? FontAttributes { get; set; } = Xamarin.Forms.FontAttributes.None;
        public double? FontSize { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public string? FontFamily { get; set; }
        public Color? BorderColor { get; set; }
        public float? BorderSize { get; set; }

        public Style ToStyleSpan()
        {
            var style = this;
            var result = new Style(typeof(Span));

            Theme.AddSetter(result.Setters, Span.FontAttributesProperty, style.FontAttributes);
            Theme.AddSetter(result.Setters, Span.FontSizeProperty, style.FontSize);
            Theme.AddSetter(result.Setters, Span.TextColorProperty, style.ForegroundColor);
            Theme.AddSetter(result.Setters, Span.BackgroundColorProperty, style.BackgroundColor);
            Theme.AddSetter(result.Setters, Span.FontFamilyProperty, style.FontFamily);

            return result;
        }

        public Style ToStyleLabel()
        {
            var style = this;
            var result = new Style(typeof(Label));

            Theme.AddSetter(result.Setters, Label.FontAttributesProperty, style.FontAttributes);
            Theme.AddSetter(result.Setters, Label.FontSizeProperty, style.FontSize);
            Theme.AddSetter(result.Setters, Label.TextColorProperty, style.ForegroundColor);
            Theme.AddSetter(result.Setters, VisualElement.BackgroundColorProperty, style.BackgroundColor);
            Theme.AddSetter(result.Setters, Label.FontFamilyProperty, style.FontFamily);

            return result;
        }

        public Style ToStyleFrame()
        {
            var style = this;
            var result = new Style(typeof(Frame));

            Theme.AddSetter(result.Setters, Frame.BackgroundColorProperty, style.BackgroundColor);
            Theme.AddSetter(result.Setters, Frame.BorderColorProperty, style.BackgroundColor);

            return result;
        }
    }
#nullable restore
}
