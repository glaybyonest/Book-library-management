using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryManagement.Data;
using LibraryManagement.Models;
using System;
using System.Linq;
using System.Windows;

namespace LibraryManagement.ViewModels
{
    public partial class AuthorEditViewModel : ObservableObject
    {
        private readonly LibraryContext _context;
        private readonly Author _author;

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private DateTime _birthDate;

        [ObservableProperty]
        private string _country;

        public AuthorEditViewModel(Author author)
        {
            _context = new LibraryContext();
            _author = author;

            if (_author.Id != 0)
            {
                FirstName = _author.FirstName;
                LastName = _author.LastName;
                BirthDate = _author.BirthDate;
                Country = _author.Country;
            }
            else
            {
                BirthDate = DateTime.Today.AddYears(-30);
            }
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Имя и фамилия обязательны.");
                return;
            }

            _author.FirstName = FirstName;
            _author.LastName = LastName;
            _author.BirthDate = BirthDate;
            _author.Country = Country;

            if (_author.Id == 0)
                _context.Authors.Add(_author);
            else
                _context.Authors.Update(_author);

            _context.SaveChanges();
            Application.Current.Windows.OfType<Views.AuthorEditWindow>().FirstOrDefault()?.DialogResult = true;
            Application.Current.Windows.OfType<Views.AuthorEditWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private void Cancel()
        {
            Application.Current.Windows.OfType<Views.AuthorEditWindow>().FirstOrDefault()?.DialogResult = false;
            Application.Current.Windows.OfType<Views.AuthorEditWindow>().FirstOrDefault()?.Close();
        }
    }
}