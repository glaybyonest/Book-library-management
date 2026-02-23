using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Windows;

namespace LibraryManagement.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly LibraryContext _context;

        [ObservableProperty]
        private ObservableCollection<Book> _books;

        [ObservableProperty]
        private ObservableCollection<Author> _authors;

        [ObservableProperty]
        private ObservableCollection<Genre> _genres;

        [ObservableProperty]
        private Author _selectedAuthorFilter;

        [ObservableProperty]
        private Genre _selectedGenreFilter;

        [ObservableProperty]
        private string _searchText;

        public MainViewModel()
        {
            _context = new LibraryContext();

            try
            {
                // Если в базе мало записей, заполняем её демо-данными с русскоязычными книгами
                Data.SeedService.SeedPopularBooksAsync(_context).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить демо-данные: {ex.Message}", "Ошибка загрузки", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                Authors = new ObservableCollection<Author>(_context.Authors.ToList());
                Genres = new ObservableCollection<Genre>(_context.Genres.ToList());
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBooks()
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            if (SelectedAuthorFilter != null)
                query = query.Where(b => b.AuthorId == SelectedAuthorFilter.Id);

            if (SelectedGenreFilter != null)
                query = query.Where(b => b.GenreId == SelectedGenreFilter.Id);

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(b => b.Title.Contains(SearchText));

            Books = new ObservableCollection<Book>(query.ToList());
        }

        [RelayCommand]
        private void AddBook()
        {
            var window = new Views.BookEditWindow(new Book());
            if (window.ShowDialog() == true)
                LoadBooks();
        }

        [RelayCommand]
        private void EditBook(Book book)
        {
            if (book == null) return;
            var window = new Views.BookEditWindow(book);
            if (window.ShowDialog() == true)
                LoadBooks();
        }

        [RelayCommand]
        private void DeleteBook(Book book)
        {
            if (book == null) return;
            if (MessageBox.Show("Удалить книгу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                LoadBooks();
            }
        }

        [RelayCommand]
        private void ManageAuthors()
        {
            var window = new Views.AuthorsWindow();
            window.ShowDialog();
            LoadData();
        }

        [RelayCommand]
        private void ManageGenres()
        {
            var window = new Views.GenresWindow();
            window.ShowDialog();
            LoadData();
        }

        partial void OnSelectedAuthorFilterChanged(Author value) => LoadBooks();
        partial void OnSelectedGenreFilterChanged(Genre value) => LoadBooks();
        partial void OnSearchTextChanged(string value) => LoadBooks();
    }
}