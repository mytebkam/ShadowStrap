using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowStrap.UI.ViewModels.Settings;

namespace ShadowStrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for ShadowStrapPage.xaml
    /// </summary>
    public partial class ShadowStrapPage
    {
        public ShadowStrapPage()
        {
            DataContext = new ShadowStrapViewModel();
            InitializeComponent();
        }
    }
}
