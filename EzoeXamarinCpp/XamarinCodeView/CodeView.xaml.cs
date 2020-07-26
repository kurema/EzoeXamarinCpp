using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace kurema.XamarinCodeView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodeView : ContentView
    {
        public CodeView()
        {
            InitializeComponent();
        }
    }
}