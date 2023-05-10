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

namespace Equipment_Client.VM
{
    public class ListScientistsVM : BaseVM
    {
        private Scientist selectedScientist;
        private List<Scientist> scientists;
        private Position selectedPosition;
        private string search = "";

        public List<Position> Positions { get; set; }
        public Position SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
                DoSearch();
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
        public CustomCommand Reset { get; set; }
        public CustomCommand AddUser { get; set; }
        public CustomCommand RemoveUser { get; set; }
        public CustomCommand EditUser { get; set; }
        public List<Scientist> Scientists
        {
            get => scientists;
            set
            {
                scientists = value;
                Signal();
            }
        }
        public Scientist SelectedScientist
        {
            get => selectedScientist;
            set
            {
                selectedScientist = value;
                Signal();
            }
        }
        private void DoSearch()
        {
            var scientists = DBInstance.GetInstance().Scientists.
                Where(s=>s.Firstname.Contains(Search) || 
                         s.Patronymic.Contains(Search) || 
                         s.Lastname.Contains(Search) || 
                         s.Login.Contains(Search)).ToList();
            if(SelectedPosition != null)
            {
                scientists = scientists.Where(s=>s.IdPosition == SelectedPosition.Id).ToList();
            }
            Scientists = scientists;
        }
        public ListScientistsVM()
        {
            Scientists = DBInstance.GetInstance().Scientists.Include("IdPositionNavigation").ToList();
            Positions = DBInstance.GetInstance().Positions.ToList();

            RemoveUser = new CustomCommand(() =>
            {
                try
                {
                    if (SelectedScientist == null)
                    {
                        MessageBox.Show("Необходимо выбрать пользователя");
                        return;
                    }
                    if (MessageBox.Show("Вы действительно хотите удалить пользователя?", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        DBInstance.GetInstance().Scientists.Remove(SelectedScientist);
                        DBInstance.GetInstance().SaveChanges();
                        Scientists = DBInstance.GetInstance().Scientists.Include("IdPositionNavigation").ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Проблема с БД");
                }

            });

            AddUser = new CustomCommand(() =>
            {
                new EditScientist(new Scientist()).ShowDialog();
                Scientists = DBInstance.GetInstance().Scientists.Include("IdPositionNavigation").ToList();
            });
            EditUser = new CustomCommand(() =>
            {
                if (SelectedScientist == null)
                {
                    MessageBox.Show("Необходимо выбрать пользователя");
                    return;
                }

                new EditScientist(SelectedScientist).ShowDialog();
                Scientists = DBInstance.GetInstance().Scientists.Include("IdPositionNavigation").ToList();
            });
            Reset = new CustomCommand(() =>
            {
                SelectedPosition = null;
                Signal(nameof(SelectedPosition));
            });
        }
    }
}
