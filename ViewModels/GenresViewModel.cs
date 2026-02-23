using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagement.Data;
using LibraryManagement.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LibraryManagement.ViewModels
{
    public partial class GenresViewModel : ObservableObject
    {
        private readonly LibraryContext _context;

        [ObservableProperty]
        private ObservableCollection<Genre> _genres;

        [ObservableProperty]
        private Genre _selectedGenre;

        public GenresViewModel()
        {
            _context = new LibraryContext();
            LoadGenres();
        }

        private void LoadGenres()
        {
            Genres = new ObservableCollection<Genre>(_context.Genres.ToList());
        }

        [RelayCommand]
        private void AddGenre()
        {
            var window = new Views.GenreEditWindow(new Genre());
            if (window.ShowDialog() == true)
                LoadGenres();
        }

        [RelayCommand]
        private void EditGenre(Genre genre)
        {
            if (genre == null) return;
            var window = new Views.GenreEditWindow(genre);
            if (window.ShowDialog() == true)
                LoadGenres();
        }

        [RelayCommand]
        private void DeleteGenre(Genre genre)
        {
            if (genre == null) return;
            if (MessageBox.Show("Удалить жанр? Все связанные книги также будут удалены.", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
    }
}