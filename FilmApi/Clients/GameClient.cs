using FilmApi.Models;
using FilmApi.Models.Games;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilmApi.Clients
{
    public class GameClient
    {
        private readonly HttpClient _client;
        private static string _address;
        private static string _apikey;
        public GameClient()
        {
            _address = Constants.GameAddress;
            _apikey = Constants.GameApiKey;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<Game> GetGameByName(string gameName)
        {
            var responce = await _client.GetAsync($"games?key={_apikey}&search={gameName}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Game>(content);

            return result;
        }
        public async Task<GameById> GetGameById(int id)
        {
            var responce = await _client.GetAsync($"games/{id}?key={_apikey}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GameById>(content);

            return result;
        }
        public async Task<Game> GetGameByGenres(string genres)
        {
            var responce = await _client.GetAsync($"games?key={_apikey}&genres={genres}");
            responce.EnsureSuccessStatusCode();
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Game>(content);

            return result;
        }
    }
}