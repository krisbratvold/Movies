using System;

namespace Movies.Models
{
    public class UnwatchedMovie
    {
        public int UnwatchedMovieId { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Poster_Path { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User Owner { get; set; }
    }
}