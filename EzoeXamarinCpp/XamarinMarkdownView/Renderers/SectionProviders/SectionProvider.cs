using kurema.XamarinMarkdownView.Themes;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace kurema.XamarinMarkdownView.Renderers.SectionProviders
{
#nullable enable
    public interface IHeadingProvider
    {
        public SectionResult GetHeading(int level,params int[] count);
    }

    public struct SectionResult
    {
        public SectionResult(string headerText, StyleSimple? customStyle)
        {
            HeaderText = headerText ?? throw new ArgumentNullException(nameof(headerText));
            CustomStyle = customStyle ?? throw new ArgumentNullException(nameof(customStyle));
        }

        public string HeaderText { get; private set; }
        public Themes.StyleSimple? CustomStyle { get; private set; }

        public static SectionResult Empty =>
            new SectionResult();

    }

    public class SectionProviderDelegate:IHeadingProvider
    {
        private Func<int, int[], SectionResult> func;

        public SectionProviderDelegate(Func<int, int[], SectionResult> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public SectionResult GetHeading(int level, params int[] count)
        {
            return func?.Invoke(level, count) ?? SectionResult.Empty;
        }
    }

    public class SectionProviderEmpty : IHeadingProvider
    {
        public SectionResult GetHeading(int level, params int[] count)
        {
            return new SectionResult("", null);
        }
    }

    public class SectionProviderText : IHeadingProvider
    {
        public static IReadOnlyList<String> PlaceHolders =>
            new string[] { "{0}", "{1}", "{2}", "{3}", "{4}", "{5}" };

        public IReadOnlyList<SectionResult> BaseHeadingResults { get => baseHeadingResults; set => baseHeadingResults = value.ToArray(); }

        private SectionResult[] baseHeadingResults = new[] {
            SectionResult.Empty, SectionResult.Empty,SectionResult.Empty,SectionResult.Empty,SectionResult.Empty,SectionResult.Empty
        };

        public SectionProviderText(SectionResult[] baseHeadingResults)
        {
            if(baseHeadingResults == null || baseHeadingResults.Length < 5)
            {
                throw new ArgumentException("Length of " + nameof(baseHeadingResults) + " must be more than 6.");
            }

            this.baseHeadingResults = baseHeadingResults;
        }

        public SectionResult GetHeading(int level, params int[] count)
        {
            return GetActualText(BaseHeadingResults[level], count);
        }

        private SectionResult GetActualText(SectionResult heading, int[] count)
        {
            string head = heading.HeaderText;
            for (int i = 0; i < 6; i++)
            {
                head.Replace(PlaceHolders[i], count.Length < i - 1 ? "1" : (count[i] + 1).ToString());
            }
            return new SectionResult(head, heading.CustomStyle);
        }
    }
}
