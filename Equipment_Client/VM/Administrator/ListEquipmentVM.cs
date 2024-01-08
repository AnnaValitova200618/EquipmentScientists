using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Administrator
{
    public class ListEquipmentVM : BaseVM
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

        public CustomCommand EditEquipment { get; set; }
        public CustomCommand Reset { get; set; }
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
        public ListEquipmentVM(Scientist_WorkerVM scientist_WorkerVM, Scientist scientist)
        {
            try
            {
                Equipments = DBInstance.GetInstance().Equipment.ToList();
                Types = DBInstance.GetInstance().Types.ToList();
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

            Reset = new CustomCommand(() =>
            {
                SelectType = null;
                Signal(nameof(SelectType));
            });


            EditEquipment = new CustomCommand(() =>
            {
                if (SelectedEquipment == null)
                {
                    MessageBox.Show("Необходимо выбрать оборудование");
                    return;
                }
                new EditEquipment(SelectedEquipment).ShowDialog();
                Equipments = DBInstance.GetInstance().Equipment.ToList();
            });
        }
    }
}
