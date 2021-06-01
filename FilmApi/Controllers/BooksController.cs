using FilmApi.Clients;
using FilmApi.Contexts;
using FilmApi.Models;
using FilmApi.Models.Book;
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
    public class BooksController : ControllerBase
    {
        private readonly BookClient _bookClient;
        private readonly ILogger<FilmsController> _logger;
        private readonly SqlController _sqlClient;

        public BooksController(ILogger<FilmsController> logger, BookClient bookClient, SqlController sqlClient)
        {
            _logger = logger;
            _bookClient = bookClient;
            _sqlClient = sqlClient;
        }

        [HttpGet("getBookByISBNExternal/{isbn}")]
        public async Task<IActionResult> GetBookByISBNdExternal(string isbn)
        {
            var book = await _bookClient.GetBookByISBN(isbn);
            if (book.title == null)
            {
                return NotFound();
            }
            try
            {
                string[] authors = new string[book.authors.Count];
                for (int i = 0; i < book.authors.Count; i++)
                {
                    authors[i] = book.authors[i].name;
                }
                var result = new BookBeautifulResponse
                {
                    title = book.title,
                    url = book.url,
                    authors = string.Join(",", authors.Select(x => x.ToString()).ToArray()),
                    publish_date = book.publish_date,
                    isbn_10 = book.identifiers.isbn_10[0]
                };
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
            
        }
        [HttpGet("GetConnectedGamesIds/{isbn}")]
        public async Task<IActionResult> GetBookConnectedGames(string isbn)
        {
            var resp = await _sqlClient.GetBookConnectedGames(isbn);
            if (resp != "404")
            {
                return Ok(resp);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("GetConnectedFilmsIds/{isbn}")]
        public async Task<IActionResult> GetConnectedBooksIds(string isbn)
        {
            var resp = await _sqlClient.GetBookConnectedFilms(isbn);
            if (resp != "404")
            {
                return Ok(resp);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("getBookByISBN/{isbn}")]
        public IActionResult GetBookByISBN(string isbn)
        {

            var book = _sqlClient.GetBookByISBN(isbn);

            if (book == null)
            {
                return NotFound();
            }

            var result = new BookBeautifulResponse
            {
                title = book.title,
                url = book.url,
                authors = book.authors,
                publish_date = book.publish_date,
                isbn_10 = book.isbn_10
            };
            return Ok(result);
        }
        [HttpGet("getAllBooks")]
        public IEnumerable<BookResponse> GetAllBooks()
        {
            return _sqlClient.GetAllBooks();
        }
        [HttpPost("createBook")]
        public async Task<HttpResponseMessage> CreateBook([FromBody] CreateBookBody body)
        {
            var bookFromBody = new BookResponse
            {
                title = body.title,
                url = body.url,
                authors = body.authors,
                publish_date = body.publish_date,
                isbn_10 = body.isbn_10
            };
            HttpResponseMessage returnMessage = new HttpResponseMessage();
            try
            {
                var film = _sqlClient.CreateBook(bookFromBody);
                await _sqlClient.SaveChanges();
                returnMessage = new HttpResponseMessage(HttpStatusCode.Created);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, "Book Added Succesfully");
            }
            catch (Exception ex)
            {
                returnMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                returnMessage.RequestMessage = new HttpRequestMessage(HttpMethod.Post, ex.ToString());
            }

            return await Task.FromResult(returnMessage);
        }
        [HttpDelete("deleteBookByISBN/{id}")]
        public async Task<IActionResult> DeleteBookByISBN(string id)
        {
            var book = _sqlClient.GetBookByISBN(id);
            if (book == null)
            {
                return NotFound();
            }
            _sqlClient.DeleteBook(book);
            await _sqlClient.SaveChanges();

            return NoContent();
        }
        [HttpPut("updateBook/{id}")]
        public async Task<IActionResult> UpdateBook(string id, BookBeautifulResponse body)
        {
            var new_book = _sqlClient.GetBookByISBN(id);
            if (new_book == null)
            {
                return NotFound();
            }

            new_book.title = body.title;
            new_book.url = body.url;
            new_book.authors = body.authors;
            new_book.publish_date = body.publish_date;
            new_book.isbn_10 = body.isbn_10;
            await _sqlClient.SaveChanges();
            return NoContent();
        }

    }
}