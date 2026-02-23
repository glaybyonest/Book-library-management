using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagement.Data;
using LibraryManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LibraryManagement.ViewModels
{
    public partial class BookEditViewModel : ObservableObject
    {
        private readonly LibraryContext _context;
        private readonly Book _book;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private int _publishYear;

        [ObservableProperty]
        private string _isbn;

        [ObservableProperty]
        private int _quantityInStock;

        [ObservableProperty]
        private ObservableCollection<Author> _authors;

        [ObservableProperty]
        private ObservableCollection<Genre> _genres;

        [ObservableProperty]
        private Author _selectedAuthor;

        [ObservableProperty]
        private Genre _selectedGenre;

        public BookEditViewModel(Book book)
        {
            _context = new LibraryContext();
            _book = book;

            Authors = new ObservableCollection<Author>(_context.Authors.ToList());
            Genres = new ObservableCollection<Genre>(_context.Genres.ToList());

            if (_book.Id != 0)
            {
                Title = _book.Title;
                PublishYear = _book.PublishYear;
                Isbn = _book.ISBN;
                QuantityInStock = _book.QuantityInStock;
                SelectedAuthor = _book.Author;
                SelectedGenre = _book.Genre;
            }
            else
            {
                PublishYear = DateTime.Now.Year;
                QuantityInStock = 1;
            }
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title) || SelectedAuthor == null || SelectedGenre == null)
            {
                MessageBox.Show("Заполните все обязательные поля (Название, Автор, Жанр).");
                return;
            }

            _book.Title = Title;
            _book.PublishYear = PublishYear;
            _book.ISBN = Isbn;
            _book.QuantityInStock = QuantityInStock;
            _book.AuthorId = SelectedAuthor.Id;
            _book.GenreId = SelectedGenre.Id;

            if (_book.Id == 0)
                _context.Books.Add(_book);
            else
                _context.Books.Update(_book);

            _context.SaveChanges();
            Application.Current.Windows.OfType<Views.BookEditWindow>().FirstOrDefault()?.DialogResult = true;
            Application.Current.Windows.OfType<Views.BookEditWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private void Cancel()
        {
            Application.Current.Windows.OfType<Views.BookEditWindow>().FirstOrDefault()?.DialogResult = false;
            Application.Current.Windows.OfType<Views.BookEditWindow>().FirstOrDefault()?.Close();
        }
    }
}