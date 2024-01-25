using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views.Administrator;
using Equipment_Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Equipment_Client.VM.Scientist_Worker
{
    public class OperatorVM : BaseVM
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
        public CustomCommand Choose { get; set; }


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
                             s.Login.Contains(Search))
                             .Where(s => s.Id != 31 && s.IdPosition != 1)
                             .ToList();

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
        public OperatorVM(ReportVM reportVM, Window window)
        {
            try
            {
                TakeListScientists();
                Positions = DBInstance.GetInstance().Positions.Where(s=>s.Id != 1).ToList();
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

            Choose = new CustomCommand(() =>
            {
                var chooseOperator = Scientists.Where(s => s.Choice).ToList();

                if(chooseOperator.Count() > 3)
                {
                    MessageBox.Show("Нельзя выбирать больше трёх операторов!");
                    return;
                }

                reportVM.SelectOperators = chooseOperator;
                window.Close();
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
                .Where(s => s.Id != 31 && s.IdPosition != 1)
                .ToList();
        }
    }
}
