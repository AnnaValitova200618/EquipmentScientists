using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Equipment_Client.VM
{
    public class ResponsibleEquipmentVM : BaseVM
    {
        private List<Equipment> equipments;
        private Models.Type selectType;

        public List<Models.Type> Types { get; set; }
        public Models.Type SelectType 
        {
            get => selectType;
            set
            {
                selectType = value;
                Signal();
            }
        }
        public List<Status> Statuses { get; set; }
        public string Status { get; set; }
        public CustomCommand Add { get; set; }
        
        public List<Equipment> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                Signal();
            }
        }
        public Equipment Equipment { get; set; } = new();

        public ResponsibleEquipmentVM(Scientist scientist)
        {
            
            GetEquipments(scientist);
            Types = DBInstance.GetInstance().Types.ToList();
            Statuses = DBInstance.GetInstance().Statuses.ToList();
            
            
            
            Add = new CustomCommand(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(Equipment.Name) ||
                       string.IsNullOrEmpty(Equipment.Weight) ||
                       string.IsNullOrEmpty(Equipment.Dimansions) ||
                       SelectType == null)
                    {
                        MessageBox.Show("Необходимо заполнить некоторые поля");
                        return;
                    }
                    Equipment.IdType = SelectType.Id;
                    Equipment.IdReponsibleScientists = scientist.Id;
                    Equipment.IdStatus = 1;
                    DBInstance.GetInstance().Equipment.Add(Equipment);
                    DBInstance.GetInstance().SaveChanges();
                    GetEquipments(scientist);
                }
                catch
                {
                    MessageBox.Show("Проблемы с БД");
                }
            });
            
        }

        public void Save(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                DBInstance.GetInstance().SaveChanges();
            }
        }

        private List<Equipment> GetEquipments(Scientist scientist)
        {
            return Equipments = CheckStatusEquipment.CheckStatus()
                .Where(s => s.IdReponsibleScientists == scientist.Id).ToList();
                
        }
    }
}
