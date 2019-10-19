using System;
using System.Collections.Generic;
using System.Text;

using Markdig.Syntax;
using Xamarin.Forms;

using Markdig.Extensions.TaskLists;
using Markdig.Extensions.Mathematics;

namespace kurema.XamarinMarkdownView.Renderers
{
    public class TaskListRenderer : XamarinFormsObjectRenderer<TaskList>
    {
        //インライン表示には対応してません。
        protected override void Write(MarkdownRenderer renderer, TaskList obj)
        {
            renderer?.AppendBlock(new CheckBox() { IsChecked = obj?.Checked ?? false });
        }
    }

    public class MathInlineRenderer : XamarinFormsObjectRenderer<MathInline>
    {
        protected override void Write(MarkdownRenderer renderer, MathInline obj)
        {
            renderer?.AppendInline(obj?.Content.ToString() ?? "", Themes.Theme.StyleId.Math);
        }
    }
}
