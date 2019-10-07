using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace kurema.XamarinMarkdownView.Themes
{
#nullable enable
    public class Theme
    {
        public Theme(Dictionary<StyleId, Xamarin.Forms.Style> styles)
        {
            Styles = styles ?? throw new ArgumentNullException(nameof(styles));
        }

        public Dictionary<StyleId, Xamarin.Forms.Style> Styles { get; } = new Dictionary<StyleId, global::Xamarin.Forms.Style>();

        public static Theme GetDefaultTheme()
        {
            string? codeFont = null;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    codeFont = "Courier";
                    break;
                case Device.Android:
                    codeFont = "monospace";
                    break;

            }

            var theme = new Theme(
                new Dictionary<StyleId, Xamarin.Forms.Style>() {
                    {
                        StyleId.Code,
                        new StyleSimple(fontSize:Device.GetNamedSize (NamedSize.Body, typeof(Span)),fontFamily:codeFont).ToStyleSpan()
                    }
                }
                );

            return theme;
        }

        public Style GetStyleFromStyleId(StyleId key)
        {
            return Styles[key];
        }

        public enum StyleId
        {
            Document,
            Code,
            CodeBlock,
            Heading1,
            Heading2,
            Heading3,
            Heading4,
            Heading5,
            Heading6,
            Image,
            Inserted,
            Marked,
            QuoteBlock,
            StrikeThrough,
            Subscript,
            Superscript,
            TaskList,
            ThematicBreak,
            Hyperlink
        }

        public static void AddSetter(IList<Setter> setters, BindableProperty property, object? value)
        {
            if (value != null && setters != null)
            {
                setters.Add(
                    new Setter()
                    {
                        Property = property,
                        Value = value
                    }
                    );
            }
        }
    }
#nullable restore
}