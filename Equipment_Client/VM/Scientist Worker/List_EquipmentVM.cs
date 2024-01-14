using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Administrator
{
    public class List_EquipmentVM : BaseVM
    {
        private List<Equipment> equipments;
        private Models.Type selectType;
        private string search = "";
        private Equipment selectedEquipment;
        private Visibility visibility = Visibility.Collapsed;
        private Visibility visibility2 = Visibility.Visible;

        public Visibility Visibility1 
        {
            get => visibility;
            set
            {
                visibility = value;
                Signal();
            }
        }
        public Visibility Visibility2 
        {
            get => visibility2;
            set
            {
                visibility2 = value;
                Signal();
            }
        }
        
        public string Search
        {
            get => search;
            set
            {
                search = value;
                DoSearch();
            }
        }
        public List<Equipment> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                Signal();

            }
        }
        public Equipment SelectedEquipment
        {
            get => selectedEquipment;
            set
            {
                selectedEquipment = value;
                Signal();
            }
        }
        public List<Models.Type> Types { get; set; }
        public Models.Type SelectType
        {
            get => selectType;
            set
            {
                selectType = value;
                DoSearch();
            }
        }
        
        public CustomCommand Reset { get; set; }
        public CustomCommand Booking { get; set; }
        public CustomCommand Visible { get; set; }
        public CustomCommand Collapsed { get; set; }
        public CustomCommand Update { get; set; }

        private void DoSearch()
        {
            try
            {

                List<Equipment> equipments = DBInstance.GetInstance().Equipment.Where(s => s.Name.Contains(Search)).ToList();
                if (SelectType != null)
                {
                    equipments = equipments.Where(s => s.IdType == SelectType.Id).ToList();
                }
                Equipments = equipments;
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

        }

        public List_EquipmentVM(Scientist_WorkerVM scientist_WorkerVM, Scientist scientist)
        {
            try
            {
                Equipments = CheckStatusEquipment.CheckStatus();
                Types = DBInstance.GetInstance().Types.ToList();
                
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

            Booking = new CustomCommand(() =>
            {
                if (SelectedEquipment == null)
                {
                    MessageBox.Show("Необходимо выбрать оборудование");
                    return;
                }
                if(SelectedEquipment.IdStatus == 2 || SelectedEquipment.IdStatus == 3 || SelectedEquipment.IdStatus == 4 )
                {
                    MessageBox.Show("Оборудование не может быть забранировано");
                    return;
                }
                scientist_WorkerVM.CurrentPage = new BookingEquipment(scientist, SelectedEquipment, scientist_WorkerVM);
            });

            Reset = new CustomCommand(() =>
            {
                SelectType = null;
                Signal(nameof(SelectType));
            });

            Visible = new CustomCommand(() =>
            {
                Visibility2 = Visibility.Collapsed;
                Visibility1 = Visibility.Visible;
                Signal(nameof(Visibility1));
                Signal(nameof(Visibility2));
            });
            Collapsed = new CustomCommand(() =>
            {
                Visibility2 = Visibility.Visible;
                Visibility1 = Visibility.Collapsed;
                Signal(nameof(Visibility1));
                Signal(nameof(Visibility2));
            });
            Update = new CustomCommand(() =>
            {
                scientist_WorkerVM.CurrentPage = new List_Equipment(scientist_WorkerVM, scientist);
                
            });
        }
    }
}
