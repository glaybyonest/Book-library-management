using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagement.Data;
using LibraryManagement.Models;
using System.Linq;
using System.Windows;

namespace LibraryManagement.ViewModels
{
    public partial class GenreEditViewModel : ObservableObject
    {
        private readonly LibraryContext _context;
        private readonly Genre _genre;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        public GenreEditViewModel(Genre genre)
        {
            _context = new LibraryContext();
            _genre = genre;

            if (_genre.Id != 0)
            {
                Name = _genre.Name;
                Description = _genre.Description;
            }
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Название жанра обязательно.");
                return;
            }

            _genre.Name = Name;
            _genre.Description = Description;

            if (_genre.Id == 0)
                _context.Genres.Add(_genre);
            else
                _context.Genres.Update(_genre);

            _context.SaveChanges();
            Application.Current.Windows.OfType<Views.GenreEditWindow>().FirstOrDefault()?.DialogResult = true;
            Application.Current.Windows.OfType<Views.GenreEditWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private void Cancel()
        {
            Application.Current.Windows.OfType<Views.GenreEditWindow>().FirstOrDefault()?.DialogResult = false;
            Application.Current.Windows.OfType<Views.GenreEditWindow>().FirstOrDefault()?.Close();
        }
    }
}