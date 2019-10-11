using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = kurema.XamarinMarkdownView.XamarinMarkdown.ToXamarinForms("hello *world*.\n# test");
        }
    }
}
