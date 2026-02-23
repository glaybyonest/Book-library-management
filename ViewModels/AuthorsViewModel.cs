using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagement.Data;
using LibraryManagement.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LibraryManagement.ViewModels
{
    public partial class AuthorsViewModel : ObservableObject
    {
        private readonly LibraryContext _context;

        [ObservableProperty]
        private ObservableCollection<Author> _authors;

        [ObservableProperty]
        private Author _selectedAuthor;

        public AuthorsViewModel()
        {
            _context = new LibraryContext();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            Authors = new ObservableCollection<Author>(_context.Authors.ToList());
        }

        [RelayCommand]
        private void AddAuthor()
        {
            var window = new Views.AuthorEditWindow(new Author());
            if (window.ShowDialog() == true)
                LoadAuthors();
        }

        [RelayCommand]
        private void EditAuthor(Author author)
        {
            if (author == null) return;
            var window = new Views.AuthorEditWindow(author);
            if (window.ShowDialog() == true)
                LoadAuthors();
        }

        [RelayCommand]
        private void DeleteAuthor(Author author)
        {
            if (author == null) return;
            if (MessageBox.Show("Удалить автора? Все связанные книги также будут удалены.", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
                LoadAuthors();
            }
        }
    }
}