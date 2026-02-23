using System.Windows;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Views
{
    public partial class BookEditWindow : Window
    {
        public BookEditWindow(Book book)
        {
            InitializeComponent();
            DataContext = new BookEditViewModel(book);
            Owner = Application.Current.MainWindow;
        }
    }
}