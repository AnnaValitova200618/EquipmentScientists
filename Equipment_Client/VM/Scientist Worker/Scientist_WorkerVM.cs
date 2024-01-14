﻿using Equipment_Client.Tools;
using Equipment_Client.Views;
using Equipment_Client.Views.Scientist_Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Equipment_Client.VM
{
    public class Scientist_WorkerVM : BaseVM
    {
        private Page currentPage;
        public CustomCommand OpenListEquipment { get; set; }
        public CustomCommand OpenListBookingEquipment { get; set; }
        public CustomCommand OpenCabinet { get; set; }
        public CustomCommand Back { get; set; } 

        public Page CurrentPage 
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }

        public Scientist_WorkerVM(System.Windows.Window window, Models.Scientist scientist)
        {
            CurrentPage = new List_Equipment(this, scientist);

            OpenListEquipment = new CustomCommand(() =>
            {
                CurrentPage = new List_Equipment(this, scientist);
            });
            OpenListBookingEquipment = new CustomCommand(() =>
            {
                CurrentPage = new ListBookingEquipment(this);
            });
            OpenCabinet = new CustomCommand(() =>
            {
                CurrentPage = new Cabinet(scientist, this);
            });
            Back = new CustomCommand(() =>
            {
                new MainWindow().Show();
                window.Close();
            });
        }
    }
}
