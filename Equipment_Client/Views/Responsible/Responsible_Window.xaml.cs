using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.VM;
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
using System.Windows.Shapes;

namespace Equipment_Client.Views
{
    /// <summary>
    /// Логика взаимодействия для Responsible_Window.xaml
    /// </summary>
    public partial class Responsible_Window : Window
    {
        public Responsible_Window(Models.Scientist scientist)
        {
            InitializeComponent();
            DataContext = new ResponsibleWindowVM(scientist, Window);
            Scientists = DBInstance.GetInstance().Scientists.ToList();

            foreach (Scientist scientist1 in Scientists)
            {
                fio = $"{scientist1.Lastname} {scientist1.Firstname.Substring(0, 1)}.{scientist1.Patronymic.Substring(0, 1)}.";
                scientist1.FIO = fio;
            }
        }
        public string fio { get; set; }
        public List<Scientist> Scientists { get; set; }

    }

}
