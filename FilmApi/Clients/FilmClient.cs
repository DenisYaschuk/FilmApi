using FilmApi.Models.Films;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilmApi.Clients
{
    public class FilmClient
    {
        private HttpClient _client;
        private static string _address;
        private static string _apikey;
        public FilmClient()
        {
            _address = Constants.FilmAddress;
            _apikey = Constants.FilmApiKey;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<Film> GetFilmByName(string filmName)
        {
            var responce = await _client.GetAsync($"search/movie?api_key={_apikey}&query={filmName}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Film>(content);

            return result;
        }
        public async Task<FilmById> GetFilmById(int id)
        {
            var responce = await _client.GetAsync($"movie/{id}?api_key={_apikey}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<FilmById>(content);

            return result;
        }
        public async Task<Film> GetFilmByGenres(string genres)
        {
            var responce = await _client.GetAsync($"discover/movie?api_key={_apikey}&with_genres={genres}&sort_by=popularity.desc");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Film>(content);

            return result;
        }
    }
}
