using System;
using System.Collections.Generic;
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

namespace KhabirovaAutoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;
            DataContext = _currentService;
            var _currentClient = Khabirova_autoserviceEntities2.GetContext().client_a_import.ToList();
            ComboClient.ItemsSource = _currentClient;
        }
        private ClientService _currentClientService = new ClientService();
        private void Savebutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var allServices = Khabirova_autoserviceEntities2.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentService.Title).ToList();
            if (allServices.Count == 0)
            {
                if (_currentService.ID == 0)
                    Khabirova_autoserviceEntities2.GetContext().Service.Add(_currentService);
                try
                {
                    Khabirova_autoserviceEntities2.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.Message.ToString());
                }
                
            }
            /*else
            {
                MessageBox.Show("Уже существует такая услуга");
            }*/
            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            if (_currentClientService.ID == 0)
                Khabirova_autoserviceEntities2.GetContext().ClientService.Add(_currentClientService);
            try
            {
                Khabirova_autoserviceEntities2.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            if (s.Length < 4 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(new char[] { ':' });
                Console.WriteLine(start);
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());
                int sum = startHour + startMin + _currentService.Duration;
                int EndHour = sum / 60;
                if (EndHour > 23)
                {
                    EndHour -= 24;
                }
                int EndMin = sum % 60;

                if (EndMin < 9)
                    s = EndHour.ToString() + ":0" + EndMin.ToString();
                else
                {
                    s = EndHour.ToString() + ":" + EndMin.ToString();
                }

                TBEnd.Text = s;
            }
        }
    }
}
