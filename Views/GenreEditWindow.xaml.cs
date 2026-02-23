using System.Windows;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Views
{
    public partial class GenreEditWindow : Window
    {
        public GenreEditWindow(Genre genre)
        {
            InitializeComponent();
            DataContext = new GenreEditViewModel(genre);
            Owner = Application.Current.MainWindow;
        }
    }
}