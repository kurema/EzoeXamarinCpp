using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EzoeXamarinCpp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarkdownView : ContentView
    {
        public MarkdownView()
        {
            InitializeComponent();
        }

        public void Load()
        {
        }
    }
}