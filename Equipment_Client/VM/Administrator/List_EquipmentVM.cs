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
        public CustomCommand AddEquipment { get; set; }
        public CustomCommand RemoveEquipment { get; set; }
        public CustomCommand EditEquipment { get; set; }
        public CustomCommand Booking { get; set; }
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

        public CustomCommand Reset { get; set; }
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
                scientist_WorkerVM.CurrentPage = new BookingEquipment(scientist);
            });

            Reset = new CustomCommand(() =>
            {
                SelectType = null;
                Signal(nameof(SelectType));
            });

            AddEquipment = new CustomCommand(() =>
            {
                new EditEquipment(new Equipment()).ShowDialog();
                Equipments = CheckStatusEquipment.CheckStatus();
            });
            EditEquipment = new CustomCommand(() =>
            {
                if (SelectedEquipment == null)
                {
                    MessageBox.Show("Необходимо выбрать оборудование");
                    return;
                }
                new EditEquipment(SelectedEquipment).ShowDialog();
                Equipments = CheckStatusEquipment.CheckStatus();
            });
            RemoveEquipment = new CustomCommand(() =>
            {
                try
                {
                    if (SelectedEquipment == null)
                    {
                        MessageBox.Show("Необходимо выбрать оборудование");
                        return;
                    }
                    if (MessageBox.Show("Вы действительно хотите удалить оборудование?", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        DBInstance.GetInstance().Equipment.Remove(SelectedEquipment);
                        DBInstance.GetInstance().SaveChanges();
                        Equipments = CheckStatusEquipment.CheckStatus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Проблема с БД");
                }

            });
        }
    }
}
