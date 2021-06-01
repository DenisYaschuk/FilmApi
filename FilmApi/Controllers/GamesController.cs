using FilmApi.Clients;
using FilmApi.Contexts;
using FilmApi.Models;
using FilmApi.Models.Films;
using FilmApi.Models.Games;
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
    public class GamesController : ControllerBase
    {
        private readonly Random rnd = new Random();
        private readonly GameClient _gameClient;
        private readonly ILogger<GamesController> _logger;
        private readonly SqlController _sqlClient;

        public GamesController(ILogger<GamesController> logger, GameClient gameClient, SqlController sqlClient)
        {
            _logger = logger;
            _gameClient = gameClient;
            _sqlClient = sqlClient;
        }
        [HttpGet("getAllGames")]
        public IEnumerable<GameResponse> GetAllGames()
        {
            return _sqlClient.GetAllGames();
        }

        [HttpGet("gameByName")]
        public async Task<IActionResult> GetGameByName([FromQuery] GameByNameParameters parameters)
        {

            var game = await _gameClient.GetGameByName(parameters.gameName);
            try
            {
                var result = new GameResponse
                {
                    id = game.results[0].id,
                    name = game.results[0].name,
                    rating = game.results[0].rating,
                    genres = string.Join(",", game.results[0].genres.Select(x => x.id.ToString()).ToArray()),
                    released = game.results[0].released,
                    background_image = game.results[0].background_image
                };

                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("GetConnectedFilmsIds/{id}")]
        public async Task<IActionResult> GetGamesConnectedFilms(int id)
        {
            var resp = await _sqlClient.GetGameConnectedFilms(id);
            if (resp != "404")
            {
                return Ok(resp);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("GetConnectedBooksIds/{id}")]
        public async Task<IActionResult> GetGamesConnectedBooks(int id)
        {
            var resp = await _sqlClient.GetGameConnectedBooks(id);
            if (resp != "404")
            {
                return Ok( resp );
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("GetGameGenre/{id}")]
        public async Task<IActionResult> GetGameGenre(int id)
        {
            var resp = await _sqlClient.GetGameGenreSql(id);
            if (resp != "404")
            {
                return Ok(resp);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetGameByGenre/{genres}")]
        public async Task<IActionResult> GetGameByGenre(string genres)
        {
            var game = await _gameClient.GetGameByGenres(genres);
            try
            {
                int random_num = rnd.Next(0, 19);
                var result = new GameResponse
                {
                    id = game.results[random_num].id,
                    name = game.results[random_num].name,
                    rating = game.results[random_num].rating,
                    genres = string.Join(",", game.results[random_num].genres.Select(x => x.id.ToString()).ToArray()),
                    released = game.results[random_num].released,
                    background_image = game.results[random_num].background_image
                };

                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }



        [HttpGet("getGameByIdExternal/{id}")]
        public async Task<IActionResult> GetFilmByIdExternal(int id)
        {

            var game = await _gameClient.GetGameById(id);

            if (game == null)
            {
                return NotFound();
            }
            int[] genres = new int[game.genres.Count];
            for (int i = 0; i < game.genres.Count; i++)
            {
                genres[i] = game.genres[i].id;
            }

            var result = new GameResponse
            {
                id = game.id,
                name = game.name,
                rating = game.rating,
                genres = string.Join(",", game.genres.Select(x => x.id.ToString()).ToArray()),
                released = game.released,
                background_image = game.background_image
            };
            return Ok(result);
        }
        [HttpGet("getGameById/{id}")]
        public IActionResult GetGameById(int id)
        {

            var game = _sqlClient.GetGameById(id);

            if (game == null)
            {
                return NotFound();
            }

            var result = new GameResponse
            {
                id = game.id,
                name = game.name,
                rating = game.rating,
                genres = game.genres,
                released = game.released,
                background_image = game.background_image
            };
            return Ok(result);
        }
        [HttpPost("createGame")]
        public async Task<HttpResponseMessage> CreateGame([FromBody] CreateGameBody body)
        {
            var gameFromBody = new GameResponse
            {
                name = body.name,
                rating = body.rating,
                genres = body.genres,
                released = body.released,
                background_image = body.background_image
            };
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            try
            {
                var film = _sqlClient.CreateGame(gameFromBody);
                await _sqlClient.SaveChanges();
                returnMessage = new HttpResponseMessage(HttpStatusCode.Created);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Game Added Succesfully");
            }
            catch (Exception ex)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, ex.ToString());
            }

            return await Task.FromResult(returnMessage);
        }
        [HttpDelete("deleteGameById/{id}")]
        public async Task<IActionResult> DeleteGameById(int id)
        {
            var game = _sqlClient.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            _sqlClient.DeleteGame(game);
            await _sqlClient.SaveChanges();

            return NoContent();
        }
        [HttpPut("updateGame/{id}")]
        public async Task<IActionResult> UpdateGame(int id, GameResponse body)
        {
            var new_game = _sqlClient.GetGameById(id);
            if (new_game == null)
            {
                return NotFound();
            }

            new_game.name = body.name;
            new_game.rating = body.rating;
            new_game.genres = body.genres;
            new_game.released = body.released;
            new_game.background_image = body.background_image;
            await _sqlClient.SaveChanges();
            return NoContent();
        }
    }
}
