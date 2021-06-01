using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models.Book
{
    public class BookResponse
    {
        [Key]
        public int id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string authors { get; set; }
        public string publish_date { get; set; }
        
        public string isbn_10 { get; set; }
    }
}
