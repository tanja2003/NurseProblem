using NurseProblem.InterfaceAdaptersLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NurseProblem
{
    /// <summary>
    /// Interaktionslogik für NewNurseWindow.xaml
    /// </summary>
    public partial class NewNurseWindow : Window
    {
        public NewNurseWindow()
        {
            InitializeComponent();
            Loaded += (_, __) =>
            {
                if (DataContext is CreateNurseViewModel vm)
                {
                    vm.RequestClose += OnRequestClose;
                }
            };
        }

        private void OnRequestClose()
        {
            Close();
        }
    }
}
