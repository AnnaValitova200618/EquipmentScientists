using Equipment_Client.VM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Equipment_Client.Views
{
    /// <summary>
    /// Логика взаимодействия для List_Equipment.xaml
    /// </summary>
    public partial class List_Equipment : Page
    {
        public List_Equipment()
        {
            InitializeComponent();
            DataContext = new List_EquipmentVM();
        }
    }
}
