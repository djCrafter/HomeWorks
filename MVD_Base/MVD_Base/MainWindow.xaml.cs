using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity.Validation;
using System.Diagnostics;
using MVD_Base.Repo;

namespace MVD_Base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MVDdb db; 


        public MainWindow()
        {
          InitializeComponent();
          Load();
        }

        private void Load()
        {
            db = new MVDdb();
            MainQueru();
           
        }

        private void MainQueru()
        {
            var query = from e in db.Employees.ToList()
                        join r in db.Ranks on e.Rank_ID equals r.Rank_ID
                        join p in db.Positions on e.Position_ID equals p.Position_ID
                        select new TableEntity1 { Employee_ID = e.Employee_ID,
                           FullName = e.FullName,
                           Age = e.Age,
                           Gender = e.Gender,
                           Adress = e.Adress,
                           Phone = e.Phone,
                           PassportDetails = e.PassportDetails,
                           Rank = r.RankName,
                           Position = p.PositionName };

            DataGridEmployees.ItemsSource = query;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Employee employee = new Employee()
                {
                    FullName = textBoxFullName.Text,
                    Adress = textBoxAdress.Text,
                    Age = int.Parse(textBoxAge.Text),
                    Gender = comboBoxMale.Text,
                    PassportDetails = textBoxPassportDetails.Text,
                    Phone = textBoxPhone.Text,
                    Rank_ID = comboBoxRanks.SelectedIndex + 1,
                    Position_ID = comboBoxPositions.SelectedIndex + 1
                };

                db.Employees.Add(employee);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    {
                        Debug.Write("Object: " + validationError.Entry.Entity.ToString());
                        Debug.Write("");
                        foreach (DbValidationError err in validationError.ValidationErrors)
                        {
                            Debug.Write(err.ErrorMessage + "");
                        }
                    }
                }

                MainQueru();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(comboBoxMale.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (DataGridEmployees.SelectedItems.Count > 0)
            {
                for (int i = 0; i < DataGridEmployees.SelectedItems.Count; i++)
                {
                    var employee = DataGridEmployees.SelectedItems[i] as TableEntity1;

                    if (employee != null)
                    {
                       Employee emp = db.Employees.First(g => g.Employee_ID == employee.Employee_ID);

                        db.Employees.Remove(emp);                                  
                    }
                }
            }
            db.SaveChanges();
            MainQueru();
        }
    }
}
