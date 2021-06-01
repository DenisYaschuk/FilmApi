using FilmApi.Models.Book;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilmApi.Clients
{
    public class BookClient
    {

        private HttpClient _client;
        private static string _address;
        public BookClient()
        {
            _address = Constants.BookAdress;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<Book> GetBookByISBN(string bookIsbn)
        {
            var responce = await _client.GetAsync($"books?bibkeys={bookIsbn}&format=json&jscmd=data");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            try
            {
                content = content.Substring(2 + bookIsbn.Length + 3, content.Length - (2 + bookIsbn.Length + 3) - 1);
            }
            catch
            {
                content = "{}";
            }
            var result = JsonConvert.DeserializeObject<Book>(content);

            return result;
        }

    }
}
