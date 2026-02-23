using System.Windows;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Views
{
    public partial class AuthorsWindow : Window
    {
        public AuthorsWindow()
        {
            InitializeComponent();
            DataContext = new AuthorsViewModel();
            Owner = Application.Current.MainWindow;
        }
    }
}