using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views;
using Equipment_Client.Views.Administrator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Administrator
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
            try
            {
                var scientists = DBInstance.GetInstance().Scientists.
                    Where(s => s.Firstname.Contains(Search) ||
                             s.Patronymic.Contains(Search) ||
                             s.Lastname.Contains(Search) ||
                             s.Login.Contains(Search)).ToList();
                if (SelectedPosition != null)
                {
                    scientists = scientists.Where(s => s.IdPosition == SelectedPosition.Id).ToList();
                }
                Scientists = scientists;
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

        }
        public ListScientistsVM()
        {
            try
            {
                TakeListScientists();
                Positions = DBInstance.GetInstance().Positions.ToList();
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }


            RemoveUser = new CustomCommand(() =>
            {
                
                    if (SelectedScientist == null)
                    {
                        MessageBox.Show("Необходимо выбрать пользователя");
                        return;
                    }
                    if(SelectedScientist.DismissalDate != null)
                    {
                        MessageBox.Show("Пользователь уже уволен");
                        return;
                    }
                    else
                    {
                        new DismissalScientist(SelectedScientist).ShowDialog();
                        TakeListScientists();
                }
                
                
            });

            
            EditUser = new CustomCommand(() =>
            {
                if (SelectedScientist == null)
                {
                    MessageBox.Show("Необходимо выбрать пользователя");
                    return;
                }

                new EditScientist(SelectedScientist).ShowDialog();
                TakeListScientists();
            });
            Reset = new CustomCommand(() =>
            {
                SelectedPosition = null;
                Signal(nameof(SelectedPosition));
            });
        }

        private void TakeListScientists()
        {
            Scientists = DBInstance.GetInstance().Scientists
                .Include("IdPositionNavigation")
                .Include("IdLaboratotyNavigation")
                .Where(s=>s.Id != 31)
                .ToList();
        }
    }
}
