using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace kurema.XamarinMarkdownView.Themes
{
#nullable enable
    public class StyleSimple
    {
        public static StyleSimple None => new StyleSimple();

        public StyleSimple(FontAttributes? fontAttributes=null, double? fontSize = null, Color? foregroundColor = null, Color? backgroundColor = null, string? fontFamily = null,
            Color? borderColor=null, float? borderSize=null)
        {
            FontAttributes = fontAttributes;
            FontSize = fontSize;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            FontFamily = fontFamily;
            BorderColor = borderColor;
            BorderSize = borderSize;
        }

        public StyleSimple(StyleSimple styleSimple)
        {
            if (styleSimple == null) return;

            FontAttributes = styleSimple.FontAttributes;
            FontSize = styleSimple.FontSize;
            ForegroundColor = styleSimple.ForegroundColor;
            BackgroundColor = styleSimple.BackgroundColor;
            FontFamily = styleSimple.FontFamily;
            BorderColor = styleSimple.BorderColor;
            BorderSize = styleSimple.BorderSize;
        }

        public FontAttributes? FontAttributes { get; set; } = Xamarin.Forms.FontAttributes.None;
        public double? FontSize { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public string? FontFamily { get; set; }
        public Color? BorderColor { get; set; }
        public float? BorderSize { get; set; }

        public static StyleSimple Combine(StyleSimple a, StyleSimple b)
        {
            return new StyleSimple()
            {
                FontAttributes = (b?.FontAttributes ?? Xamarin.Forms.FontAttributes.None) | (a?.FontAttributes ?? Xamarin.Forms.FontAttributes.None),
                FontSize = b?.FontSize ?? a?.FontSize,
                ForegroundColor = b?.ForegroundColor ?? a?.ForegroundColor,
                //BackgroundColor = b?.BackgroundColor ?? a?.BackgroundColor,
                FontFamily = b?.FontFamily ?? a?.FontFamily,
                //BorderColor = b?.BorderColor ?? a?.BorderColor,
                //BorderSize = b?.BorderSize ?? a?.BorderSize
            };
        }

        public static StyleSimple Combine(params StyleSimple[] styles)
        {
            var result = new StyleSimple();
            foreach(var item in styles)
            {
                result = Combine(result, item);
            }
            return result;
        }

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

        public Style ToStyleBox()
        {
            var style = this;
            var result = new Style(typeof(BoxView));

            Theme.AddSetter(result.Setters, BoxView.BackgroundColorProperty, style.BackgroundColor);
            Theme.AddSetter(result.Setters, BoxView.ColorProperty, style.ForegroundColor);
            Theme.AddSetter(result.Setters, BoxView.HeightProperty, style.BorderSize);
            Theme.AddSetter(result.Setters, BoxView.WidthProperty, style.BorderSize);

            return result;
        }
    }
#nullable restore
}
