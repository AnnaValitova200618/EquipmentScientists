using Equipment_Client.DB;
using Equipment_Client.Models;
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
    /// Логика взаимодействия для ListEquipmentPage.xaml
    /// </summary>
    public partial class ListEquipmentPage : Page
    {
        public ListEquipmentPage(Scientist_WorkerVM scientist_WorkerVM, Models.Scientist scientist)
        {
            InitializeComponent();
            DataContext = new ListEquipmentVM(scientist_WorkerVM, scientist);
            Scientists = DBInstance.GetInstance().Scientists.ToList();

            foreach (Scientist scientist1 in Scientists)
            {

                fio = $"{scientist1.Lastname} {scientist1.Firstname.Substring(0, 1)}.{scientist1.Patronymic.Substring(0, 1)}.";

                if (scientist1.Id == 31)
                {
                    fio = scientist1.Lastname;
                }

                scientist1.FIO = fio;
            }
        }
        public string fio { get; set; }
        public List<Scientist> Scientists { get; set; }
    }
}
