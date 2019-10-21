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

        public static Theme GetDefaultTheme(ThemeColor color)
        {
            string? codeFont = null;
            double fontSizeHead = 28;
            double fontSizeBody = 17;
            try
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        codeFont = "Courier";
                        break;
                    case Device.Android:
                        codeFont = "monospace";
                        break;
                    case Device.UWP:
                    case Device.WPF:
                        //Fix me.
                        codeFont = "Arial";
                        break;
                    default:
                        //ToDo: Fix
                        codeFont = "monospace";
                        break;
                }

                fontSizeHead = Device.GetNamedSize(NamedSize.Title, typeof(Span));
                fontSizeBody = Device.GetNamedSize(NamedSize.Body, typeof(Span));
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Is this real device?");
            }
            double fontSizeHeaderMinusBody = fontSizeHead - fontSizeBody;

            var theme = new Theme(
                new Dictionary<StyleId, StyleSimple>() {
                    {
                        StyleId.Document,
                        new StyleSimple(backgroundColor:color.BackgroundColor,foregroundColor:color.TextColor,margin:new Thickness(20))
                    },
                    {
                        StyleId.Code,
                        new StyleSimple(fontSize:fontSizeBody,fontFamily:codeFont,foregroundColor:color.TextColor,backgroundColor:color.CodeBackground)
                    },
                    {
                        StyleId.CodeBlock,
                        new StyleSimple(fontSize:fontSizeBody,fontFamily:codeFont,foregroundColor:color.TextColor,backgroundColor:color.CodeBackground,margin:new Thickness(10),lineBreakMode:LineBreakMode.CharacterWrap)
                    },
                    {
                        StyleId.Math,
                        new StyleSimple(fontSize:fontSizeBody,foregroundColor:color.TextColor,backgroundColor:color.CodeBackground)
                    },
                    {
                        StyleId.MathBlock,
                        new StyleSimple(fontSize:fontSizeBody,foregroundColor:color.TextColor,backgroundColor:color.CodeBackground,margin:new Thickness(10),lineBreakMode:LineBreakMode.CharacterWrap)
                    },
                    {
                        StyleId.Heading1,
                        new StyleSimple(fontSize:fontSizeHead,
                        foregroundColor:color.TextColor,borderColor:color.SeparatorColor,borderSize:1,
                        fontAttributes:FontAttributes.Bold)
                    },
                    {
                        StyleId.Heading2,
                        new StyleSimple(fontSize:fontSizeBody+fontSizeHeaderMinusBody*5.0/7.0,
                        foregroundColor:color.TextColor,borderColor:color.SeparatorColor,borderSize:0.5f,
                        fontAttributes:FontAttributes.Bold)
                    },
                    {
                        StyleId.Heading3,
                        new StyleSimple(fontSize:fontSizeBody+fontSizeHeaderMinusBody*4.0/7.0,
                        foregroundColor:color.TextColor,borderColor:color.SeparatorColor,
                        fontAttributes:FontAttributes.Bold)
                    },
                    {
                        StyleId.Heading4,
                        new StyleSimple(fontSize:fontSizeBody+fontSizeHeaderMinusBody*3.0/7.0,
                        foregroundColor:color.TextColor,borderColor:color.SeparatorColor,
                        fontAttributes:FontAttributes.Bold)
                    },
                    {
                        StyleId.Heading5,
                        new StyleSimple(fontSize:fontSizeBody+fontSizeHeaderMinusBody*2.0/7.0,
                        foregroundColor:color.TextColor,borderColor:color.SeparatorColor,
                        fontAttributes:FontAttributes.Bold)
                    },
                    {
                        StyleId.Heading6,
                        new StyleSimple(fontSize:fontSizeBody+fontSizeHeaderMinusBody*1.0/7.0,
                        foregroundColor:color.TextColor,borderColor:color.SeparatorColor,
                        fontAttributes:FontAttributes.Bold)
                    },
                    {
                        StyleId.Inserted,
                        new StyleSimple(fontSize:fontSizeBody,
                        foregroundColor:color.TextColor,textDecorations:TextDecorations.Underline)
                    },
                    {
                        StyleId.Marked,
                        new StyleSimple(foregroundColor:color.TextColor,backgroundColor:color.MarkedColor)
                    },
                    {
                        StyleId.Paragraph,
                        new StyleSimple(fontSize:fontSizeBody,
                        foregroundColor:color.TextColor)
                    },
                    {
                        StyleId.QuoteBlock,
                        new StyleSimple(fontSize:fontSizeBody,
                        foregroundColor:color.QuoteTextColor)
                    },
                    {
                        StyleId.QuoteBlockBar,
                        new StyleSimple(borderColor:color.QuoteBorderColor,borderSize:1)
                    },
                    {
                        StyleId.StrikeThrough,
                        new StyleSimple(textDecorations:TextDecorations.Strikethrough)
                    },
                    {
                        //Not supported.
                        StyleId.Subscript,
                        new StyleSimple(fontSize:fontSizeBody*0.5)
                    },
                    {
                        //Not supported.
                        StyleId.Superscript,
                        new StyleSimple(fontSize:fontSizeBody*0.5)
                    },
                    {
                        StyleId.ThematicBreak,
                        new StyleSimple(borderColor:color.SeparatorColor,borderSize:1.0f)
                    },
                    {
                        StyleId.Hyperlink,
                        new StyleSimple(foregroundColor:color.AccentColor)
                    },
                    {
                        StyleId.ListItem,
                        new StyleSimple(foregroundColor:color.TextColor)
                    },
                    {
                        StyleId.ListBullet,
                        new StyleSimple(foregroundColor:color.TextColor)
                    },
                    {
                        StyleId.ListNumber,
                        new StyleSimple(foregroundColor:color.TextColor)
                    },
                    {
                        StyleId.HtmlBlock,
                        new StyleSimple()
                    },
                    {
                        StyleId.EmphasisBold,
                        new StyleSimple(fontAttributes:FontAttributes.Bold)
                    },
                    {
                        StyleId.EmphasisItalic,
                        new StyleSimple(fontAttributes:FontAttributes.Italic)
                    },
                    {
                        StyleId.None,
                        new StyleSimple()
                    },
                }
                );

            return theme;
        }

        public StyleSimple GetStyleFromStyleId(StyleId key)
        {
            if (!Styles.ContainsKey(key)) return StyleSimple.None;
            return Styles[key];
        }

        public enum StyleId
        {
            Document,
            Code,
            CodeBlock,
            Math,
            MathBlock,
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
            QuoteBlockBar,
            StrikeThrough,
            Subscript,
            Superscript,
            ThematicBreak,
            Hyperlink,
            ListItem,
            ListBullet,
            ListNumber,
            HtmlBlock,
            EmphasisBold,
            EmphasisItalic,
            TableCellBody,
            TableCellHeader,
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

    public struct ThemeColor : IEquatable<ThemeColor>
    {
        public ThemeColor(Color backgroundColor, Color accentColor, Color textColor, Color codeBackground, Color separatorColor, Color quoteTextColor, Color quoteBorderColor,Color markedColor)
        {
            BackgroundColor = backgroundColor;
            AccentColor = accentColor;
            TextColor = textColor;
            CodeBackground = codeBackground;
            SeparatorColor = separatorColor;
            QuoteTextColor = quoteTextColor;
            QuoteBorderColor = quoteBorderColor;
            MarkedColor = markedColor;
        }

        public Color BackgroundColor { get; }
        public Color AccentColor { get; }
        public Color TextColor { get; }
        public Color CodeBackground { get; }
        public Color SeparatorColor { get; }
        public Color QuoteTextColor { get; }
        public Color QuoteBorderColor { get; }
        public Color MarkedColor { get; }

        public override bool Equals(object? obj)
        {
            return obj is ThemeColor color && Equals(color);
        }

        public bool Equals(ThemeColor other)
        {
            return EqualityComparer<Color>.Default.Equals(BackgroundColor, other.BackgroundColor) &&
                   EqualityComparer<Color>.Default.Equals(AccentColor, other.AccentColor) &&
                   EqualityComparer<Color>.Default.Equals(TextColor, other.TextColor) &&
                   EqualityComparer<Color>.Default.Equals(CodeBackground, other.CodeBackground) &&
                   EqualityComparer<Color>.Default.Equals(SeparatorColor, other.SeparatorColor) &&
                   EqualityComparer<Color>.Default.Equals(QuoteTextColor, other.QuoteTextColor) &&
                   EqualityComparer<Color>.Default.Equals(QuoteBorderColor, other.QuoteBorderColor) &&
                   EqualityComparer<Color>.Default.Equals(MarkedColor, other.MarkedColor);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BackgroundColor, AccentColor, TextColor, CodeBackground, SeparatorColor, QuoteTextColor, QuoteBorderColor, MarkedColor);
        }

        public static bool operator ==(ThemeColor left, ThemeColor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ThemeColor left, ThemeColor right)
        {
            return !(left == right);
        }
    }

    public static class ThemeColors
    {
        public static ThemeColor Light =>
            new ThemeColor(
                backgroundColor: Color.FromHex("#ffffff"),
                accentColor: Color.FromHex("#0366d6"), 
                textColor: Color.FromHex("#24292e"), 
                codeBackground: Color.FromHex("#f6f8fa"), 
                separatorColor: Color.FromHex("#eaecef"), 
                quoteTextColor: Color.FromHex("#6a737d"), 
                quoteBorderColor: Color.FromHex("#dfe2e5"),
                markedColor:Color.FromHex("#FFFF00"));

        public static ThemeColor Dark =>
            new ThemeColor(
                backgroundColor: Color.FromHex("#2b303b"),
                accentColor: Color.FromHex("#d08770"),
                textColor: Color.FromHex("#eff1f5"),
                codeBackground: Color.FromHex("#4f5b66"),
                separatorColor: Color.FromHex("#65737e"),
                quoteTextColor: Color.FromHex("#a7adba"),
                quoteBorderColor: Color.FromHex("#a7adba"),
                markedColor: Color.FromHex("#CCCC00"));
    }

#nullable restore
}