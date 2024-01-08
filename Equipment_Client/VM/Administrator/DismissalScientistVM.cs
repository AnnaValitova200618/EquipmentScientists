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
    public class DismissalScientistVM : BaseVM
    {
        public Scientist Scientist { get; set; }
        public CustomCommand Save { get; set; }
        public List<Equipment> Equipments { get; set; }
        public DismissalScientistVM(Scientist selectedScientist, Window window)
        {
            Scientist = selectedScientist;
            Equipments = DBInstance.GetInstance().Equipment.ToList();

            Save = new CustomCommand(() =>
            {
                if(Scientist.DismissalDate == null)
                {
                    MessageBox.Show("Необходимо выбрать дату");
                    return;
                }
                else
                {
                    if(Scientist.IdPosition == 2)
                    {
                        foreach(var equipment in Equipments)
                        {
                            if(equipment.IdReponsibleScientists == Scientist.Id)
                            {
                                equipment.IdReponsibleScientists = 31;
                            }
                        }
                    }
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Данные успешно сохранены");
                    window.Close();
                }
            });
        }
    }
}
