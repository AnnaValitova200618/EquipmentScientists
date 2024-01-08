using Equipment_Client.VM.Administrator;
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

namespace Equipment_Client.Views.Administrator
{
    /// <summary>
    /// Логика взаимодействия для DismissalScientist.xaml
    /// </summary>
    public partial class DismissalScientist : Window
    {
        public DismissalScientist(Models.Scientist selectedScientist)
        {
            InitializeComponent();
            DataContext = new DismissalScientistVM(selectedScientist, window);
        }
    }
}
