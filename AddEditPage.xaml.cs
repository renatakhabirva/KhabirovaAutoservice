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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        public bool a = false;
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
            {
                a = true;
                _currentService = SelectedService;
            }
                DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");
            if (_currentService.Cost <= 0)
                errors.AppendLine("Укажите стоимость услуги");
            if (string.IsNullOrWhiteSpace(_currentService.Discount.ToString()))
                errors.AppendLine("Укажите скидку");
            if (string.IsNullOrWhiteSpace(_currentService.Duration.ToString())) 
                errors.AppendLine("Укажите длительность услуги");
           
            if (_currentService.Cost <= 0)
            {
                MessageBox.Show("Стоимость введена неверно");
                return;
            }
            if (_currentService.Discount < 0 || _currentService.Discount > 100)
            {
                MessageBox.Show("Скидка введена неверно");
                return;
            }
            
            if(_currentService.Duration > 240 || _currentService.Duration <0)
                errors.AppendLine("Длительность не может быть больше 240 и меньше 0");
            
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var allServices = Khabirova_autoserviceEntities2.GetContext().Service.ToList();
            allServices = allServices.Where(p=> p.Title == _currentService.Title).ToList();
            
            if (allServices.Count == 0 || a)
            {
                if (_currentService.ID == 0)
                {
                    Khabirova_autoserviceEntities2.GetContext().Service.Add(_currentService);
                }
                
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
            else
                {
                    MessageBox.Show("Уже существует такая услуга");
                }
        }
        

    private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
