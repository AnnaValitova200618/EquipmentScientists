﻿using Equipment_Client.VM;
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

namespace Equipment_Client.Views
{
    /// <summary>
    /// Логика взаимодействия для Scientists_Window.xaml
    /// </summary>
    public partial class Administrator_Window : Window
    {
        public Scientist_WorkerVM scientist_WorkerVM;
        public Administrator_Window(Models.Scientist scientist)
        {
            
            InitializeComponent();
            DataContext = new Administrator_WindowVM(Window, scientist, scientist_WorkerVM);
        }
    }
}
