using System.Windows;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Views
{
    public partial class GenresWindow : Window
    {
        public GenresWindow()
        {
            InitializeComponent();
            DataContext = new GenresViewModel();
            Owner = Application.Current.MainWindow;
        }
    }
}