using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Xamarin.Forms;

namespace kurema.XamarinMarkdownView.Themes
{
#nullable enable
    public class StyleSimple
    {
        public static StyleSimple None => new StyleSimple();

        public StyleSimple(FontAttributes? fontAttributes=null, double? fontSize = null, Color? foregroundColor = null, Color? backgroundColor = null, string? fontFamily = null,
            Color? borderColor=null, float? borderSize=null, TextDecorations? textDecorations=Xamarin.Forms.TextDecorations.None,
            Thickness? margin=null)
        {
            FontAttributes = fontAttributes;
            FontSize = fontSize;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            FontFamily = fontFamily;
            BorderColor = borderColor;
            BorderSize = borderSize;
            TextDecorations = textDecorations;
            Margin = margin;
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
            TextDecorations = styleSimple.TextDecorations;
            Margin = styleSimple.Margin;
        }

        public FontAttributes? FontAttributes { get; set; } = Xamarin.Forms.FontAttributes.None;
        public double? FontSize { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public string? FontFamily { get; set; }
        public Color? BorderColor { get; set; }
        public float? BorderSize { get; set; }
        public TextDecorations? TextDecorations { get; set; } = Xamarin.Forms.TextDecorations.None;
        public Thickness? Margin { get; set; }

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
                //BorderSize = b?.BorderSize ?? a?.BorderSize,
                TextDecorations = (b?.TextDecorations ?? Xamarin.Forms.TextDecorations.None) | (a?.TextDecorations ?? Xamarin.Forms.TextDecorations.None)
            };
        }

        public static StyleSimple CombineLast(StyleSimple a, StyleSimple b)
        {
            return new StyleSimple()
            {
                FontAttributes = (b?.FontAttributes ?? Xamarin.Forms.FontAttributes.None) | (a?.FontAttributes ?? Xamarin.Forms.FontAttributes.None),
                FontSize = b?.FontSize ?? a?.FontSize,
                ForegroundColor = b?.ForegroundColor ?? a?.ForegroundColor,
                BackgroundColor = b?.BackgroundColor,
                FontFamily = b?.FontFamily ?? a?.FontFamily,
                BorderColor = b?.BorderColor,
                BorderSize = b?.BorderSize,
                TextDecorations = (b?.TextDecorations ?? Xamarin.Forms.TextDecorations.None) | (a?.TextDecorations ?? Xamarin.Forms.TextDecorations.None),
                Margin = b?.Margin
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

        public static StyleSimple CombineLast(params StyleSimple[] styles)
        {
            if (styles.Length == 0) return new StyleSimple();
            else if (styles.Length == 1) return styles[0];
            var last = styles.Last();
            var other = styles.ToList();
            other.RemoveAt(other.Count - 1);
            return CombineLast(Combine(other.ToArray()), last);
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
            Theme.AddSetter(result.Setters, Span.TextDecorationsProperty, style.TextDecorations);

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
            Theme.AddSetter(result.Setters, Label.TextDecorationsProperty, style.TextDecorations);
            //Theme.AddSetter(result.Setters, Label.MarginProperty, style.Margin);

            return result;
        }

        public Style ToStyleFrame()
        {
            var style = this;
            var result = new Style(typeof(Frame));

            Theme.AddSetter(result.Setters, Frame.BackgroundColorProperty, style.BackgroundColor);
            Theme.AddSetter(result.Setters, Frame.BorderColorProperty, style.BackgroundColor);
            Theme.AddSetter(result.Setters, Frame.MarginProperty, style.Margin);

            return result;
        }

        public Style ToStyleBox()
        {
            var style = this;
            var result = new Style(typeof(BoxView));

            Theme.AddSetter(result.Setters, BoxView.BackgroundColorProperty, style.BackgroundColor);
            Theme.AddSetter(result.Setters, BoxView.ColorProperty, style.ForegroundColor);
            Theme.AddSetter(result.Setters, BoxView.HeightRequestProperty, style.BorderSize);
            Theme.AddSetter(result.Setters, BoxView.WidthRequestProperty, style.BorderSize);
            Theme.AddSetter(result.Setters, BoxView.MarginProperty, style.Margin);

            return result;
        }
    }
#nullable restore
}
