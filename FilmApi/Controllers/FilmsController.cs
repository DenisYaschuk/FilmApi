using FilmApi.Clients;
using FilmApi.Contexts;
using FilmApi.Models;
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
    public class FilmsController : ControllerBase
    {
        private readonly Random rnd = new Random();
        private readonly FilmClient _filmClient;
        private readonly ILogger<FilmsController> _logger;
        private readonly SqlController _sqlClient;


        public FilmsController(ILogger<FilmsController> logger, FilmClient filmClient, SqlController sqlClient)
        {
            _logger = logger;
            _filmClient = filmClient;
            _sqlClient = sqlClient;
        }
        [HttpGet("GetFilmGenre/{id}")]
        public async Task<IActionResult> GetFilmGenre(int id)
        {
            var resp = await _sqlClient.GetFilmGenreSql(id);
            if (resp != "404")
            {
                return Ok(resp);
            }
            else
            {
                return NotFound();
            }
            
        }
        [HttpGet("GetConnectedGamesIds/{id}")]
        public async Task<IActionResult> GetFilmsConnectedGames(int id)
        {
            var resp = await _sqlClient.GetFilmConnectedGames(id);
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
        public async Task<IActionResult> GetConnectedBooksIds(int id)
        {
            var resp = await _sqlClient.GetFilmConnectedBooks(id);
            if ( resp != "404")
            {
                return Ok(resp);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetFilmByGenre/{genres}")]
        public async Task<IActionResult> GetFilmByGenre(string genres)
        {
            var film = await _filmClient.GetFilmByGenres(genres);
            try
            {
                int random_num = rnd.Next(0, 19);
                var result = new FilmResponse
                {
                    id = film.results[random_num].id,
                    original_title = film.results[random_num].original_title,
                    overview = film.results[random_num].overview,
                    genre_ids = string.Join(",", film.results[random_num].genre_ids.Select(x => x.ToString()).ToArray()),
                    release_date = film.results[random_num].release_date,
                    vote_average = film.results[random_num].vote_average,
                    poster_path = film.results[random_num].poster_path
                };

                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }


        [HttpGet("getFilmByIdExternal/{id}")]
        public async Task<IActionResult> GetFilmByIdExternal(int id)
        {

            var film = await _filmClient.GetFilmById(id);

            if (film == null)
            {
                return NotFound();
            }
            int[] genres = new int[film.genres.Count];
            for(int i = 0; i < film.genres.Count; i++)
            {
                genres[i] = film.genres[i].id;
            }

            var result = new FilmResponse
            {
                id = film.id,
                original_title = film.original_title,
                overview = film.overview,
                genre_ids = string.Join(",", genres.Select(x => x.ToString()).ToArray()),
                release_date = film.release_date,
                vote_average = film.vote_average,
                poster_path = film.poster_path
            };
            return Ok(result);
        }

        [HttpGet("getFilmById/{id}")]
        public IActionResult GetFilmById(int id)
        {

            var film = _sqlClient.GetFilmById(id);

            if (film == null)
            {
                return NotFound();
            }

            var result = new FilmResponse
            {
                id = film.id,
                original_title = film.original_title,
                overview = film.overview,
                genre_ids = film.genre_ids,
                release_date = film.release_date,
                vote_average = film.vote_average,
                poster_path = film.poster_path
            };
            return Ok(result);
        }

        [HttpGet("filmByName")]
        public async Task<IActionResult> GetFilmByName([FromQuery] FilmByNameParameters parameters)
        {
            

            var film = await _filmClient.GetFilmByName(parameters.filmName);
            try
            {
                var result = new FilmResponse
                {
                    id = film.results[0].id,
                    original_title = film.results[0].original_title,
                    overview = film.results[0].overview,
                    genre_ids = string.Join(",", film.results[0].genre_ids.Select(x => x.ToString()).ToArray()),
                    release_date = film.results[0].release_date,
                    vote_average = film.results[0].vote_average,
                    poster_path = film.results[0].poster_path
                };

                return Ok(result);
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpGet("getAllFilms")]
        public IEnumerable<FilmResponse> GetAllFilms()
        {
            return  _sqlClient.GetAllFilms();
        }

        [HttpPost("createFilm")]
        public async Task<HttpResponseMessage> CreateFilm([FromBody] CreateFilmBody body)
        {
            var filmFromBody = new FilmResponse
            {
                genre_ids = body.genre_ids,
                original_title = body.original_title,
                overview = body.overview,
                release_date = body.release_date,
                vote_average = body.vote_average,
                poster_path = body.poster_path
            };
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            try
            {
                var film = _sqlClient.CreateFilm(filmFromBody);
                await _sqlClient.SaveChanges();
                returnMessage = new HttpResponseMessage(HttpStatusCode.Created);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Film Added Succesfully");
            }
            catch (Exception ex)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, ex.ToString());
            }

            return await Task.FromResult(returnMessage);
        }

        [HttpDelete("deleteFilmById/{id}")]
        public async Task<IActionResult> DeleteFilmById(int id)
        {
            var film = _sqlClient.GetFilmById(id);
            if (film == null)
            {
                return NotFound();
            }
            _sqlClient.DeleteFilm(film);
            await _sqlClient.SaveChanges();

            return NoContent();
        }

        [HttpPut("updateFilm/{id}")]
        public async Task<IActionResult> UpdateFilm(int id, FilmResponse body)
        {
            var new_film = _sqlClient.GetFilmById(id);
            if (new_film == null){
                return NotFound();
            }
  
            new_film.original_title = body.original_title;
            new_film.overview = body.overview;
            new_film.release_date = body.release_date;
            new_film.genre_ids = body.genre_ids;
            new_film.vote_average = body.vote_average;
            new_film.poster_path = body.poster_path;
            await _sqlClient.SaveChanges();
            return NoContent();
        }

    }
}