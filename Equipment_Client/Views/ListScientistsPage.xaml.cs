﻿using Equipment_Client.Models;
using Equipment_Client.VM;
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
    /// Логика взаимодействия для ListScientistsPage.xaml
    /// </summary>
    public partial class ListScientistsPage : Page
    {
        public ListScientistsPage()
        {
            InitializeComponent();
            DataContext = new ListScientistsVM();
        }
    }
}
