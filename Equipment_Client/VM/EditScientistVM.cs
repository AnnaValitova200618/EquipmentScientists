using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Equipment_Client.VM
{
    public class EditScientistVM : BaseVM
    {
        private Position selectPosition;
        private Visibility visibility = Visibility.Visible;

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                Signal();
            }
        }
        public CustomCommand Save { get; set; }
        public Scientist Scientist { get; set; }
        public Position SelectPosition
        {
            get => selectPosition;
            set
            {
                selectPosition = value;
                Signal();
            }
        }
        public List<Position> Positions { get; set; }
        public EditScientistVM(Scientist selectscientist, Window window)
        {
            try
            {
                Scientist = selectscientist;
                SelectPosition = Scientist.IdPositionNavigation;
                Positions = DBInstance.GetInstance().Positions.ToList();
                if (Scientist.Id != 0)
                {
                    Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }
            
            Save = new CustomCommand(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(Scientist.Firstname) ||
                    string.IsNullOrEmpty(Scientist.Patronymic) ||
                    SelectPosition == null ||
                    string.IsNullOrEmpty(Scientist.Lastname) ||
                    string.IsNullOrEmpty(Scientist.Login) ||
                    string.IsNullOrEmpty(Scientist.Password))
                    {
                        MessageBox.Show("Необходимо заполнить все поля");
                        return;
                    }
                    if (Scientist.Id == 0)
                    {
                        var user = DBInstance.GetInstance().Scientists.FirstOrDefault(s => s.Login == Scientist.Login);

                        if (user != null)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует");
                            return;
                        }
                        DBInstance.GetInstance().Scientists.Add(
                            new Scientist
                            {
                                Login = Scientist.Login,
                                Firstname = Scientist.Firstname,
                                Patronymic = Scientist.Patronymic,
                                Lastname = Scientist.Lastname,
                                Password = HashPass.GetPass(Scientist.Password),
                                IdPosition = SelectPosition.Id
                            });
                    }
                    Scientist.IdPosition = SelectPosition.Id;
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Оке");
                    window.Close();
                }
                catch
                {
                    MessageBox.Show("Проблема с БД");
                }

            });
        }
    }
}
