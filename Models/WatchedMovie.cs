using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class WatchedMovie
    {
        public int WatchedMovieId { get; set; }
        public string Title { get; set; }
        public int MovieId { get; set; }
        public string Poster_Path { get; set; }
        [Required]
        [Range(0,10, ErrorMessage ="must be 10 or lower")]
        [Display(Name="Rating")]
        public Decimal PersonalRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User Owner { get; set; }
    }
}