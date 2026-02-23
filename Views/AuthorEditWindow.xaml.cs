using System.Windows;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Views
{
    public partial class AuthorEditWindow : Window
    {
        public AuthorEditWindow(Author author)
        {
            InitializeComponent();
            DataContext = new AuthorEditViewModel(author);
            Owner = Application.Current.MainWindow;
        }
    }
}