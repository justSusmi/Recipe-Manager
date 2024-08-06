using Recipte_Management.HomePage;
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

namespace Recipte_Management
{
    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class HomePage_View : Page
    {
        public HomePage_View()
        {
            InitializeComponent();

            this.DataContext = new HomePage_ViewModel();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as HomePage_ViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedRecipe = e.AddedItems[0] as Recipe_Model;
            }
        }
    }
}
