using FilmApi.Models.Book;
using FilmApi.Models.Films;
using FilmApi.Models.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using FilmApi.Models.Favourite;

namespace FilmApi.Contexts
{
    public class SqlController
    {
        private static AppDbContext _context;
        private static SqlDbManualRepo _dbCon;

        public SqlController(AppDbContext context, SqlDbManualRepo dbCon)
        {
            _context = context;
            _dbCon = dbCon;
        }


        public async Task<String> GetGameConnectedFilms(int id)
        {
            if (await _dbCon.IsConnect())
            {
                string result = "404";
                try
                {
                    string query = $"SELECT * FROM films_and_games_connections WHERE game_id = {id}";
                    var cmd = new MySqlCommand(query, _dbCon.Connection);
                    var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = (await reader.GetFieldValueAsync<Int32>(0)).ToString();
                    }
                    await reader.CloseAsync();
                    await _dbCon.Close();
                    return result;

                }
                catch
                {
                    return "404";
                }
            }
            return "404";
        }
        public async Task<String> GetGameConnectedBooks(int id)
        {
            if (await _dbCon.IsConnect())
            {
                string result = "404";
                try
                {
                    string query = $"SELECT * FROM games_and_books_connections WHERE game_id = {id}";
                    var cmd = new MySqlCommand(query, _dbCon.Connection);
                    var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = (await reader.GetFieldValueAsync<String>(1)).ToString();
                    }

                    await reader.CloseAsync();
                    await _dbCon.Close();
                    return result;

                }
                catch
                {
                    return "404";
                }
            }
            return "404";
        }
        public async Task<String> GetFilmConnectedBooks(int id)
        {
            if (await _dbCon.IsConnect())
            {
                string result = "404";
                try
                {
                    string query = $"SELECT * FROM films_and_books_connections WHERE film_id = {id}";
                    var cmd = new MySqlCommand(query, _dbCon.Connection);
                    var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = (await reader.GetFieldValueAsync<String>(1)).ToString();
                    }
                    await reader.CloseAsync();
                    await _dbCon.Close();
                    return result;

                }
                catch
                {
                    return "404";
                }
            }
            return "404";
        }
        public async Task<String> GetBookConnectedGames(string id)
        {
            if (await _dbCon.IsConnect())
            {
                string result = "404";
                try
                {
                    string query = $"SELECT * FROM games_and_books_connections WHERE book_id = {id}";
                    var cmd = new MySqlCommand(query, _dbCon.Connection);
                    var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        result = (await reader.GetFieldValueAsync<Int32>(0)).ToString();
                    }
                    await reader.CloseAsync();
                    await _dbCon.Close();
                    return result;
                }
                catch
                {
                    return "404";
                }
            }
            return "404";
        }

        public async Task<String> GetFilmConnectedGames(int id)
        {
            string result = "404";
            if (await _dbCon.IsConnect())
            {
                try
                {
                    //460465
                    string query = $"SELECT * FROM films_and_games_connections WHERE film_id = {id}";
                    
                    var cmd = new MySqlCommand(query, _dbCon.Connection);

                    var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        
                        result = (await reader.GetFieldValueAsync<Int32>(1)).ToString();
                    }
                    await reader.CloseAsync();
                    await _dbCon.Close();

                    return result;

                }
                catch
                {
                    return result;
                }
            }
            return result;
        }
        public async Task<String> GetBookConnectedFilms(string id)
        {
            if (await _dbCon.IsConnect())
            {
                string result = "404";
                try
                {
                    string query = $"SELECT * FROM films_and_books_connections WHERE book_id = {id}";
                    var cmd = new MySqlCommand(query, _dbCon.Connection);
                    var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = (await reader.GetFieldValueAsync<Int32>(0)).ToString();
                    }
                    await reader.CloseAsync();
                    await _dbCon.Close();
                    return result;

                }
                catch
                {
                    return "404";
                }
            }
            return "404";
        }
        public async Task<String> GetFilmGenreSql(int id)
        {
            if (await _dbCon.IsConnect())
            {
                string result = "404";
                try
                {
                string query = $"SELECT * FROM films_genres WHERE id = {id}";
                    var cmd = new MySqlCommand(query, _dbCon.Connection);
                    var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = (await reader.GetFieldValueAsync<String>(1)).ToString();
                    }
                    await reader.CloseAsync();
                    await _dbCon.Close();
                    return result;

                }
                catch
                {
                    return "404";
                }
            }
            return "404";
        }
        public async Task<String> GetGameGenreSql(int id)
        {
            if (await _dbCon.IsConnect())
            {
                string result = "404";
                try
                {
                    string query = $"SELECT * FROM games_genres WHERE id = {id}";
                    var cmd = new MySqlCommand(query, _dbCon.Connection);
                    var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        result = (await reader.GetFieldValueAsync<String>(1)).ToString();
                    }
                    await reader.CloseAsync();
                    await _dbCon.Close();
                    return result;

                }
                catch
                {
                    return "404";
                }
            }
            return "404";
        }

        public IEnumerable<FilmResponse> GetAllFilms()
        {
            return _context.Films.ToList();
        }

        public IEnumerable<GameResponse> GetAllGames()
        {
            return _context.Games.ToList();
        }
        public IEnumerable<BookResponse> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public FilmResponse GetFilmById(int id)
        {
            return _context.Films.FirstOrDefault(p => p.id == id);
        }

        public GameResponse GetGameById(int id)
        {
            return _context.Games.FirstOrDefault(p => p.id == id);
        }
        public BookResponse GetBookByISBN(string isbn_10)
        {
            return _context.Books.FirstOrDefault(p => p.isbn_10 == isbn_10);
        }

        public async Task<FilmResponse> CreateFilm(FilmResponse film)
        {
            await _context.Films.AddAsync(film);
            return film;
        }

        public async Task<GameResponse> CreateGame(GameResponse game)
        {
            await _context.Games.AddAsync(game);
            return game;
        }
        public async Task<BookResponse> CreateBook(BookResponse book)
        {
            await _context.Books.AddAsync(book);
            return book;
        }
        public FilmResponse DeleteFilm(FilmResponse film)
        {
            if (film == null)
            {
                throw new ArgumentNullException(nameof(film));
            }
            _context.Films.Remove(film);
            return film;
        }

        public GameResponse DeleteGame(GameResponse game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }
            _context.Games.Remove(game);
            return game;
        }
        public BookResponse DeleteBook(BookResponse book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            _context.Books.Remove(book);
            return book;
        }

        public async Task<FavouriteBook> CreateFavouriteBook(FavouriteBook data)
        {
            await _context.FavouriteBooks.AddAsync(data);
            return data;
        }
        public FavouriteBook DelteFavouriteBook(FavouriteBook data)
        {
             _context.FavouriteBooks.Remove(data);
            return data;
        }
        public IEnumerable<FavouriteBook> GetFavouriteBooksByChatId(string chatId)
        {
            return _context.FavouriteBooks.Where(p => p.ChatId == chatId);
        }
        public async Task<FavouriteBook> GetFavouriteBookByChatIdAndIsbn(FavouriteBookBody dataToDelete)
        {
            return await _context.FavouriteBooks.FirstOrDefaultAsync(p => (p.ChatId==dataToDelete.ChatId && p.book_id==dataToDelete.book_id));
        }

        public async Task<FavouriteGame> CreateFavouriteGame(FavouriteGame data)
        {
            await _context.FavouriteGames.AddAsync(data);
            return data;
        }
        public FavouriteGame DelteFavouriteBook(FavouriteGame data)
        {
            _context.FavouriteGames.Remove(data);
            return data;
        }
        public IEnumerable<FavouriteGame> GetFavouriteGamesByChatId(string chatId)
        {
            return _context.FavouriteGames.Where(p => p.ChatId == chatId);
        }
        public async Task<FavouriteGame> GetFavouriteGameByChatIdAndGameId(FavouriteGameBody dataToDelete)
        {
            return await _context.FavouriteGames.FirstOrDefaultAsync(p => (p.ChatId == dataToDelete.ChatId && p.game_id == dataToDelete.game_id));
        }

        public async Task<FavouriteFilm> CreateFavouriteFilm(FavouriteFilm data)
        {
            await _context.FavouriteFilms.AddAsync(data);
            return data;
        }
        public FavouriteFilm DelteFavouriteFilm(FavouriteFilm data)
        {
            _context.FavouriteFilms.Remove(data);
            return data;
        }
        public IEnumerable<FavouriteFilm> GetFavouriteFilmsByChatId(string chatId)
        {
            return _context.FavouriteFilms.Where(p => p.ChatId == chatId);
        }
        public async Task<FavouriteFilm> GetFavouriteFilmByChatIdAndFilmId(FavouriteFilmBody dataToDelete)
        {
            return await _context.FavouriteFilms.FirstOrDefaultAsync(p => (p.ChatId == dataToDelete.ChatId && p.film_id == dataToDelete.film_id));
        }





        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
