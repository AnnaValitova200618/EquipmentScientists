using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Administrator
{
    public class EditEquipmentVM : BaseVM
    {
        private Scientist selectScientist;
        private Models.Type selectType;

        public List<Scientist> Scientists { get; set; }
        public Scientist SelectScientist
        {
            get => selectScientist;
            set
            {
                selectScientist = value;
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
                Signal();
            }
        }
        public Equipment Equipment { get; set; }
        public CustomCommand Save { get; set; }
        public EditEquipmentVM(Equipment selectedEquipment, Window window)
        {
            try
            {
                Equipment = selectedEquipment;
                Types = DBInstance.GetInstance().Types.ToList();
                Scientists = DBInstance.GetInstance().Scientists.Where(s => s.IdPosition == 2 && s.DismissalDate == null).ToList();
                SelectScientist = Equipment.IdReponsibleScientistsNavigation;
                SelectType = Equipment.IdTypeNavigation;
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

            Save = new CustomCommand(() =>
            {
                if (string.IsNullOrEmpty(Equipment.Name) ||
                   string.IsNullOrEmpty(Equipment.Dimansions) ||
                   string.IsNullOrEmpty(Equipment.Weight) ||
                   SelectScientist == null ||
                   SelectType == null)
                {
                    MessageBox.Show("Необходимо заполнить все данные");
                    return;
                }
                try
                {
                    Equipment.IdReponsibleScientists = SelectScientist.Id;
                    Equipment.IdType = SelectType.Id;

                    if (Equipment.Id == 0)
                    {
                        Equipment.IdStatus = 1;
                        DBInstance.GetInstance().Equipment.Add(Equipment);
                    }

                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Успех!");
                    window.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка с БД");
                    return;
                }
            });
        }
    }
}
