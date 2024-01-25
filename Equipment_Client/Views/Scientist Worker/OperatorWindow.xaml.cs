using Azure;
using Equipment_Client.VM.Scientist_Worker;
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

namespace Equipment_Client.Views.Scientist_Worker
{
    /// <summary>
    /// Логика взаимодействия для OperatorWindow.xaml
    /// </summary>
    public partial class OperatorWindow : Window
    {
        public OperatorWindow(ReportVM reportVM)
        {
            InitializeComponent();
            DataContext = new OperatorVM(reportVM, window);
        }
    }
}
