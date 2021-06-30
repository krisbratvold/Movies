using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LoginPage.Controllers
{
    public class HomeController : Controller
    {

        private MoviesContext db;
        public HomeController(MoviesContext context)
        {
            db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Success");
            }

            return View("Index");
        }
        [HttpGet("/home")]
        public async Task<IActionResult> Success()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ApiHelper.InitializeClient();
            ViewBag.movies = await new MovieProcessor().GetPopMovies(1);
            return View("Dashboard");
        }
        [HttpGet("/home/next/page")]
        public async Task<IActionResult> Next()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int page = (int)HttpContext.Session.GetInt32("Page");
            page += 1;
            HttpContext.Session.SetInt32("Page", page);
            ApiHelper.InitializeClient();
            ViewBag.movies = await new MovieProcessor().GetPopMovies(page);
            return View("Dashboard");
        }
        [HttpGet("/home/prev/page")]
        public async Task<IActionResult> Prev()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            int page = (int)HttpContext.Session.GetInt32("Page");
            page -= 1;
            HttpContext.Session.SetInt32("Page", page);
            ApiHelper.InitializeClient();
            ViewBag.movies = await new MovieProcessor().GetPopMovies(page);
            return View("Dashboard");
        }

        [HttpGet("/home/view/{movieId}")]
        public async Task<IActionResult> ViewMovie(int movieId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ApiHelper.InitializeClient();
            ViewBag.SingleMovie = await new MovieProcessor().GetDetails(movieId);
            return View("Details");
        }

        [HttpGet("/home/view/watchlist")]
        public IActionResult ViewUnwatched()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Watchlist = db.UnwatchedMovies.ToList();
            return View("Watchlist");
        }

        [HttpGet("/home/view/watched")]
        public IActionResult ViewWatched()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Watched = db.WatchedMovies.OrderByDescending(m=>m.PersonalRating).ToList();
            return View("Watched");
        }

        [HttpPost("/home/add/watchlist")]
        public async Task<IActionResult> AddToWatch(UnwatchedMovie newMovie)
        {
            if (ModelState.IsValid)
            {
                if (db.UnwatchedMovies.Where(u => u.UserId == HttpContext.Session.GetInt32("UserId")).Any(m => m.MovieId == newMovie.MovieId))
                {
                    ModelState.AddModelError("validate", "Movie already on Watchlist");
                }
                if(db.WatchedMovies.Where(u => u.UserId == HttpContext.Session.GetInt32("UserId")).Any(m => m.MovieId == newMovie.MovieId))
                {
                    ModelState.AddModelError("validate", "You've already seen this movie");
                }
            }
            if (ModelState.IsValid == false)
            {
                int movieId = newMovie.MovieId;
                ApiHelper.InitializeClient();
                ViewBag.SingleMovie = await new MovieProcessor().GetDetails(movieId);
                return View("Details");
            }
            newMovie.UserId = (int)HttpContext.Session.GetInt32("UserId");
            db.UnwatchedMovies.Add(newMovie);
            db.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpPost("/home/add/watched")]
        public IActionResult AddToWatched(WatchedMovie newMovie, int movieId)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Watchlist = db.UnwatchedMovies.ToList();
                return View("Watchlist");
            }
            UnwatchedMovie unwatched = db.UnwatchedMovies.FirstOrDefault(m=>m.MovieId == movieId);
            db.UnwatchedMovies.Remove(unwatched);
            newMovie.UserId = (int)HttpContext.Session.GetInt32("UserId");
            db.WatchedMovies.Add(newMovie);
            db.SaveChanges();
            return RedirectToAction("ViewUnwatched");
        }

        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "is taken");
                }
            }
            if (ModelState.IsValid == false)
            {
                return View("Index");
            }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("Name", newUser.FirstName);
            HttpContext.Session.SetInt32("Page", 1);
            return RedirectToAction("Success");
        }

        [HttpPost("/login")]
        public IActionResult Login(LoginUser newLogin)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index");

            }
            User dbUser = db.Users.FirstOrDefault(user => user.Email == newLogin.LoginEmail);

            if (dbUser == null)
            {
                ModelState.AddModelError("LoginEmail", "incorrect credntials");
                return View("Index");
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(newLogin, dbUser.Password, newLogin.LoginPassword);

            if (pwCompareResult == 0)
            {
                ModelState.AddModelError("LoginEmail", "incorrect credntials");
                return View("Index");
            }
            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            HttpContext.Session.SetString("Name", dbUser.FirstName);
            HttpContext.Session.SetInt32("Page", 1);
            return RedirectToAction("Success");
        }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
