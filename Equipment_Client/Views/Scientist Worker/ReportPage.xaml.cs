﻿using Equipment_Client.Models;
using Equipment_Client.VM;
using Equipment_Client.VM.Responsible;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Equipment_Client.Views.Scientist_Worker
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        

        public ReportPage(Models.Booking selectBooking, Models.Scientist scientist, VM.Scientist_WorkerVM scientist_WorkerVM)
        {
            InitializeComponent();
            DataContext = new ReportVM(selectBooking, scientist, scientist_WorkerVM);
        }

        public ReportPage(Report selectReport, Scientist scientist, Scientist_WorkerVM scientist_WorkerVM)
        {
            InitializeComponent();
            DataContext = new ReportVM(selectReport, scientist, scientist_WorkerVM);
            
        }

        public ReportPage(Report selectReport, Scientist scientist, ResponsibleWindowVM responsibleWindowVM)
        {
            InitializeComponent();
            DataContext = new ReportVM(selectReport, scientist, responsibleWindowVM);
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            
            ((ReportVM)DataContext).RemoveImage((byte[])button.Tag);
        }
        
    }
}