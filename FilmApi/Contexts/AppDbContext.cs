using FilmApi.Models.Book;
using FilmApi.Models.Favourite;
using FilmApi.Models.Films;
using FilmApi.Models.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;


namespace FilmApi.Contexts
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseMySql("server=filmgamebookapi.mariadb.database.azure.com;port=3306;user=sql5415893@filmgamebookapi;password=iPft6nxgal;database=filmsgamesbooks",
                new MySqlServerVersion(new Version(8, 0, 11)))
                .UseLoggerFactory(LoggerFactory.Create(b => b
                .AddConsole()
                .AddFilter(level => level >= LogLevel.Information)))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<FilmResponse> Films { get; set; }
        public DbSet<GameResponse> Games { get; set; }
        public DbSet<BookResponse> Books { get; set; }
        public DbSet<FavouriteBook> FavouriteBooks { get; set; }
        public DbSet<FavouriteGame> FavouriteGames { get; set; }
        public DbSet<FavouriteFilm> FavouriteFilms { get; set; }
    }
}
