using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Сервис для заполнения базы демо-данными (популярные русскоязычные книги).
    /// </summary>
    public static class SeedService
    {
        private static readonly IReadOnlyList<(string Title, string AuthorFullName, string Genre, int Year)> SeedBooks =
            new List<(string, string, string, int)>
            {
                ("Война и мир", "Лев Толстой", "Роман", 1869),
                ("Анна Каренина", "Лев Толстой", "Роман", 1877),
                ("Преступление и наказание", "Фёдор Достоевский", "Роман", 1866),
                ("Идиот", "Фёдор Достоевский", "Роман", 1869),
                ("Братья Карамазовы", "Фёдор Достоевский", "Роман", 1880),
                ("Мастер и Маргарита", "Михаил Булгаков", "Роман", 1966),
                ("Собачье сердце", "Михаил Булгаков", "Повесть", 1925),
                ("Тихий Дон", "Михаил Шолохов", "Роман", 1940),
                ("Отцы и дети", "Иван Тургенев", "Роман", 1862),
                ("Евгений Онегин", "Александр Пушкин", "Роман в стихах", 1833),
                ("Капитанская дочка", "Александр Пушкин", "Повесть", 1836),
                ("Мёртвые души", "Николай Гоголь", "Поэма", 1842),
                ("Ревизор", "Николай Гоголь", "Пьеса", 1836),
                ("Белая гвардия", "Михаил Булгаков", "Роман", 1925),
                ("Доктор Живаго", "Борис Пастернак", "Роман", 1957),
                ("Двенадцать стульев", "Ильф и Петров", "Роман", 1928),
                ("Золотой телёнок", "Ильф и Петров", "Роман", 1931),
                ("Мы", "Евгений Замятин", "Антиутопия", 1921),
                ("Пикник на обочине", "Аркадий и Борис Стругацкие", "Научная фантастика", 1972),
                ("Трудно быть богом", "Аркадий и Борис Стругацкие", "Научная фантастика", 1964),
                ("Чистый код", "Роберт Мартин", "IT / программирование", 2008),
                ("CLR via C#", "Джеффри Рихтер", "IT / программирование", 2012),
                ("Гарри Поттер и философский камень", "Джоан Роулинг", "Фэнтези", 1997),
                ("Метро 2033", "Дмитрий Глуховский", "Фэнтези / постапокалипсис", 2005),
                ("Ночной дозор", "Сергей Лукьяненко", "Городское фэнтези", 1998)
            };

        public static async Task SeedPopularBooksAsync(LibraryContext context, int limit = 100)
        {
            try
            {
                // Если в базе уже достаточно книг, не подгружаем демо-данные
                var existingCount = await context.Books.CountAsync().ConfigureAwait(false);
                if (existingCount >= Math.Min(limit, SeedBooks.Count))
                {
                    return;
                }

                foreach (var seed in SeedBooks)
                {
                    var title = seed.Title;
                    var authorName = seed.AuthorFullName;
                    var genreName = seed.Genre;
                    var publishYear = seed.Year;

                    // Разбиваем имя автора на имя/фамилию (грубое, но для демо достаточно)
                    var authorFirstName = authorName;
                    var authorLastName = string.Empty;
                    var parts = (authorName ?? string.Empty)
                        .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 0)
                    {
                        authorFirstName = parts[0];
                        authorLastName = parts.Length > 1 ? parts[1] : string.Empty;
                    }

                    // Ищем/создаём автора
                    var author = await context.Authors
                        .FirstOrDefaultAsync(a => a.FirstName == authorFirstName && a.LastName == authorLastName)
                        .ConfigureAwait(false);

                    if (author == null)
                    {
                        author = new Author
                        {
                            FirstName = authorFirstName,
                            LastName = authorLastName,
                            BirthDate = DateTime.Today.AddYears(-30),
                            Country = "Россия"
                        };
                        await context.Authors.AddAsync(author).ConfigureAwait(false);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                    }

                    // Ищем/создаём жанр
                    var genre = await context.Genres
                        .FirstOrDefaultAsync(g => g.Name == genreName)
                        .ConfigureAwait(false);

                    if (genre == null)
                    {
                        genre = new Genre
                        {
                            Name = genreName,
                            Description = "Русскоязычный жанр (демо-данные)"
                        };
                        await context.Genres.AddAsync(genre).ConfigureAwait(false);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                    }

                    // Не добавляем дубликаты по (Title, Author)
                    var exists = await context.Books
                        .AnyAsync(b => b.Title == title && b.AuthorId == author.Id)
                        .ConfigureAwait(false);

                    if (exists)
                        continue;

                    var book = new Book
                    {
                        Title = title,
                        PublishYear = publishYear,
                        ISBN = null,
                        QuantityInStock = 1,
                        AuthorId = author.Id,
                        GenreId = genre.Id
                    };

                    await context.Books.AddAsync(book).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось заполнить базу демо-данными (русские книги).\n\n" +
                    $"Текст ошибки:\n{ex.Message}",
                    "Ошибка загрузки данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}

