using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Equipment_Client.VM
{
    public class Administrator_WindowVM : BaseVM
    {
        private Page currentPage;
        public Page CurrentPage 
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }
        public CustomCommand Back { get; set; }
        public CustomCommand OpenScientists { get; set; }
        public CustomCommand OpenEquipment { get; set; }
        public Administrator_WindowVM(Window window, Scientist scientist)
        {
            CurrentPage = new ListScientistsPage();

            Back = new CustomCommand(() =>
            {
                new MainWindow().Show();
                window.Close();
            });
            OpenScientists = new CustomCommand(() =>
            {
                CurrentPage = new ListScientistsPage();
            });
            OpenEquipment = new CustomCommand(() =>
            {
                CurrentPage = new ListEquipmentPage();
            });
        }
    }
}
