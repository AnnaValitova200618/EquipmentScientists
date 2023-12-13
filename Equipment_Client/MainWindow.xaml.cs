using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.VM;
using Microsoft.EntityFrameworkCore;
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

namespace Equipment_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM(window, password);

            Scientists = DBInstance.GetInstance().Scientists.ToList();

            foreach (Scientist scientist in Scientists)
            {
                fio = $"{scientist.Lastname} {scientist.Firstname.Substring(0, 1)}.{scientist.Patronymic.Substring(0, 1)}.";
                scientist.FIO = fio;
            }
        }
        public string fio { get; set; }
        public List<Scientist> Scientists { get; set; }
        
    }
}
