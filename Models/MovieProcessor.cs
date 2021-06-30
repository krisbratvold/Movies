using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class MovieProcessor
    {
        public async Task<MultipleMovie> GetPopMovies(int page)
        {
            string url = $"https://api.themoviedb.org/3/movie/popular?api_key=601a0c0649a7340cf78c3fbefb1f804b&language=en-US&page={page}";
            

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    MultipleMovie movies = await response.Content.ReadAsAsync<MultipleMovie>();
                    return movies;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase); 
                }
            }
        }
        public async Task<Details> GetDetails(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}?api_key=601a0c0649a7340cf78c3fbefb1f804b&language=en-US";
            

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            if (response.IsSuccessStatusCode)
                {
                    Details movie = await response.Content.ReadAsAsync<Details>();
                    return movie;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase); 
                }
            }
        }
    }
