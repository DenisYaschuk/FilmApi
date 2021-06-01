using FilmApi.Clients;
using FilmApi.Contexts;
using FilmApi.Models;
using FilmApi.Models.Book;
using FilmApi.Models.Favourite;
using FilmApi.Models.Films;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace FilmApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavouritesController : ControllerBase
    {
        private readonly ILogger<FilmsController> _logger;
        private readonly SqlController _sqlClient;

        public FavouritesController(ILogger<FilmsController> logger, SqlController sqlClient)
        {
            _logger = logger;
            _sqlClient = sqlClient;
        }
        [HttpGet("GetAllFavouriteBooks/{ChatId}")]
        public IEnumerable<FavouriteBook> GetAllFavouriteBooks(string ChatId)
        {
            return _sqlClient.GetFavouriteBooksByChatId(ChatId);
        }
        [HttpPost("createFavouriteBook")]
        public async Task<IActionResult> CreateBook([FromBody] FavouriteBookBody body)
        {
            try
            {
                var bookFromBody = new FavouriteBook
                {
                    ChatId = body.ChatId,
                    book_id = body.book_id
                };
                var inDb = await _sqlClient.GetFavouriteBookByChatIdAndIsbn(body);
                if (inDb != null)
                {
                    return BadRequest();
                }
                await _sqlClient.CreateFavouriteBook(bookFromBody);
                await _sqlClient.SaveChanges();
                return Ok(bookFromBody);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("deleteFavouriteBook")]
        public async Task<IActionResult> DeleteFavouriteBook([FromBody] FavouriteBookBody dataToDelete)
        {
            var book = await _sqlClient.GetFavouriteBookByChatIdAndIsbn(dataToDelete);
            if (book == null)
            {
                return NotFound();
            }
            _sqlClient.DelteFavouriteBook(book);
            await _sqlClient.SaveChanges();

            return NoContent();
        }





        [HttpGet("GetAllFavouriteGames/{ChatId}")]
        public IEnumerable<FavouriteGame> GetAllFavouriteGames(string ChatId)
        {
            return _sqlClient.GetFavouriteGamesByChatId(ChatId);
        }
        [HttpPost("createFavouriteGame")]
        public async Task<IActionResult> CreateFavouriteGame([FromBody] FavouriteGameBody body)
        {
            try
            {
                var gameFromBody = new FavouriteGame
                {
                    ChatId = body.ChatId,
                    game_id = body.game_id
                };
                var inDb = await _sqlClient.GetFavouriteGameByChatIdAndGameId(body);
                if (inDb != null)
                {
                    return BadRequest();
                }
                await _sqlClient.CreateFavouriteGame(gameFromBody);
                await _sqlClient.SaveChanges();
                return Ok(gameFromBody);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("deleteFavouriteGame")]
        public async Task<IActionResult> DeleteFavouriteGame([FromBody] FavouriteGameBody dataToDelete)
        {
            var game = await _sqlClient.GetFavouriteGameByChatIdAndGameId(dataToDelete);
            if (game == null)
            {
                return NotFound();
            }
            _sqlClient.DelteFavouriteBook(game);
            await _sqlClient.SaveChanges();

            return NoContent();
        }




        [HttpGet("GetAllFavouriteFilms/{ChatId}")]
        public IEnumerable<FavouriteFilm> GetAllFavouriteFilms(string ChatId)
        {
            return _sqlClient.GetFavouriteFilmsByChatId(ChatId);
        }
        [HttpPost("createFavouriteFilm")]
        public async Task<IActionResult> CreateFavouriteFilm([FromBody] FavouriteFilmBody body)
        {
            try
            {
                var filmFromBody = new FavouriteFilm
                {
                    ChatId = body.ChatId,
                    film_id = body.film_id
                };
                var inDb = await _sqlClient.GetFavouriteFilmByChatIdAndFilmId(body);
                if (inDb != null)
                {
                    return BadRequest();
                }
                await _sqlClient.CreateFavouriteFilm(filmFromBody);
                await _sqlClient.SaveChanges();
                return Ok(filmFromBody);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("deleteFavouriteFilm")]
        public async Task<IActionResult> DeleteFavouriteFilm([FromBody] FavouriteFilmBody dataToDelete)
        {
            var film = await _sqlClient.GetFavouriteFilmByChatIdAndFilmId(dataToDelete);
            if (film == null)
            {
                return NotFound();
            }
            _sqlClient.DelteFavouriteFilm(film);
            await _sqlClient.SaveChanges();

            return NoContent();
        }


    }
}