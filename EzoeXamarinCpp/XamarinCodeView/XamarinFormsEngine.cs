using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Highlight.Patterns;
using Highlight.Engines;

using Xamarin.Forms;

namespace kurema.XamarinCodeView
{
    public class XamarinFormsEngine : Highlight.Engines.EngineGeneric<Label, Span[]>
    {
        protected override Label HighlightUsingRegex(Definition definition, string input)
        {
            return CombineUsingRegex(definition, input, (s) => {
                var result = new Label() {
                };
                foreach(var item in s)
                {
                    if (item == null) continue;
                    foreach (var item2 in item)
                    {
                        result.FormattedText.Spans.Add(item2);
                    }
                }
                return result;
            });
        }

        protected override Span[] ProcessBlockPatternMatch(Definition definition, BlockPattern pattern, Match match)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            return new[] { new Span() {
                Text=match.Value,
                Style=GetSpanStyle(pattern.Style)
            } };
        }

        protected override Span[] ProcessMarkupPatternMatch(Definition definition, MarkupPattern pattern, Match match)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var result = new List<Span>();
            result.Add(new Span()
            {
                Text = match.Groups["openTag"].Value,
                Style = GetSpanStyle(pattern.BracketColors, pattern.Style?.Font),
            });
            result.Add(new Span()
            {
                Text = match.Groups["ws1"].Value,
            });
            result.Add(new Span()
            {
                Text = match.Groups["tagName"].Value,
                Style = GetSpanStyle(pattern.Style),

            });
            if (pattern.HighlightAttributes)
            {
                var highlightedAttributes = ProcessMarkupPatternAttributeMatches(definition, pattern, match);
                result.AddRange(highlightedAttributes);
            }
            result.Add(new Span()
            {
                Text = match.Groups["ws5"].Value,
            });
            result.Add(new Span()
            {
                Text = match.Groups["closeTag"].Value,
                Style = GetSpanStyle(pattern.BracketColors, pattern.Style?.Font),
            });
            return result.ToArray();
        }

        protected Span[] ProcessMarkupPatternAttributeMatches(Definition definition, MarkupPattern pattern, Match match)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var result = new List<Span>();

            for(int i = 0; i < match.Groups["attribute"].Captures.Count; i++)
            {
                result.Add(new Span()
                {
                    Text = match.Groups["ws2"].Captures[i].Value,
                });
                result.Add(new Span()
                {
                    Text = match.Groups["attribName"].Value,
                    Style = GetSpanStyle(pattern.AttributeNameColors, pattern.Style?.Font),
                });
                if (string.IsNullOrEmpty(match.Groups["attribValue"].Captures[i].Value)) continue;
                result.Add(new Span()
                {
                    Text = match.Groups["attribValue"].Value,
                    Style = GetSpanStyle(pattern.AttributeValueColors, pattern.Style?.Font),
                });
            }

            return result.ToArray();
        }

        protected override Span[] ProcessSimpleConvert(string input)
        {
            return new[] { new Span() { Text = input } };
        }

        protected override Span[] ProcessWordPatternMatch(Definition definition, WordPattern pattern, Match match)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            return new[] { new Span() {
                Text=match.Value,
                Style=GetSpanStyle(pattern.Style)
            } };
        }


        public static Xamarin.Forms.Style GetSpanStyle(Highlight.Patterns.Style style)
            => GetSpanStyle(style?.Colors, style?.Font);

        public static Xamarin.Forms.Style GetSpanStyle(ColorPair colorPair,System.Drawing.Font font)
        {
            static void AddSetter(IList<Setter> setters, BindableProperty property, object value)
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

            var result = new Xamarin.Forms.Style(typeof(Span));

            if (colorPair != null)
            {
                AddSetter(result.Setters, Span.BackgroundColorProperty, colorPair.BackColor);
                AddSetter(result.Setters, Span.TextColorProperty, colorPair.ForeColor);
            }
            if (font != null)
            {
                AddSetter(result.Setters, Span.FontFamilyProperty, font.FontFamily);
                AddSetter(result.Setters, Span.FontAttributesProperty,
                    (font.Bold ? FontAttributes.Bold : FontAttributes.None) | (font.Italic ? FontAttributes.Italic : FontAttributes.None)
                    );
            }

            return result;
        }
    }
}
