using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace kurema.XamarinMarkdownView.Themes
{
#nullable enable
    public class Theme
    {
        public Theme(Dictionary<StyleId, StyleSimple> styles)
        {
            Styles = styles ?? throw new ArgumentNullException(nameof(styles));
        }

        public Dictionary<StyleId, StyleSimple> Styles { get; } = new Dictionary<StyleId, StyleSimple>();

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
                new Dictionary<StyleId, StyleSimple>() {
                    {
                        StyleId.Code,
                        new StyleSimple(fontSize:Device.GetNamedSize (NamedSize.Body, typeof(Span)),fontFamily:codeFont)
                    }
                }
                );

            return theme;
        }

        public StyleSimple GetStyleFromStyleId(StyleId key)
        {
            return Styles[key];
        }

        public enum StyleId
        {
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
            Paragraph,
            QuoteBlock,
            StrikeThrough,
            Subscript,
            Superscript,
            TaskList,
            ThematicBreak,
            Hyperlink,
            ListItem,
            ListBullet,
            ListNumber,
            HtmlBlock,
            EmphasisBold,
            EmphasisItalic,
            None
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