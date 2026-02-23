using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace LibraryManagement.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }

        public LibraryContext()
        {
            try
            {
                // Гарантируем, что файловая база данных и схема созданы
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось инициализировать базу данных SQLite.\n\n" +
                    "Файл базы данных будет создан автоматически рядом с исполняемым файлом " +
                    "(library.db). Убедитесь, что у приложения есть права на запись в папку.\n\n" +
                    $"Текст ошибки:\n{ex.Message}",
                    "Ошибка базы данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                throw;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Файловая база SQLite в рабочей директории приложения
            optionsBuilder.UseSqlite("Data Source=library.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary keys
            modelBuilder.Entity<Author>().HasKey(a => a.Id);
            modelBuilder.Entity<Genre>().HasKey(g => g.Id);
            modelBuilder.Entity<Book>().HasKey(b => b.Id);

            // Required fields and max length
            modelBuilder.Entity<Author>().Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Author>().Property(a => a.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Author>().Property(a => a.Country).HasMaxLength(50);

            modelBuilder.Entity<Genre>().Property(g => g.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Genre>().Property(g => g.Description).HasMaxLength(200);

            modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Book>().Property(b => b.ISBN).HasMaxLength(20);
            modelBuilder.Entity<Book>().Property(b => b.PublishYear).IsRequired();
            modelBuilder.Entity<Book>().Property(b => b.QuantityInStock).IsRequired();

            // Relationships and cascade delete
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}