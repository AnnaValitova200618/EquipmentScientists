using Equipment_Client.VM.Responsible;
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
    /// Логика взаимодействия для ResponsibleEquipment.xaml
    /// </summary>
    public partial class ResponsibleEquipment : Page
    {
        ResponsibleEquipmentVM vm;
        public ResponsibleEquipment(Models.Scientist scientist)
        {
            InitializeComponent();
            vm = new ResponsibleEquipmentVM(scientist);
            DataContext = vm;
        }

        private void Save(object sender, DataGridCellEditEndingEventArgs e)
        {
            vm.Save(e);
        }

        
    }
}
