using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Movie_API.Models;
using Newtonsoft.Json;

namespace Movie_API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        [HttpGet]
        [Route("{movieId:int}")]
        public ActionResult<IEnumerable<Movie>> Get(int movieId)
        {
            System.IO.TextReader mReader = new System.IO.StreamReader("Data/metadata.csv");
            var movieReader = new CsvReader((IParser)mReader);
            var records = movieReader.GetRecords<Movie>();

            var movies = records.Where(r => r.MovieId == movieId);

            if(movies == null){
                return NotFound();
            };

            return records.ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<MovieStat>> Stats()
        {
            System.IO.TextReader reader = new System.IO.StreamReader("Data/metadata.csv");
            var csvReader = new CsvReader((IParser)reader);
            var movieViews = csvReader.GetRecords<MovieView>();

            System.IO.TextReader mReader = new System.IO.StreamReader("Data/metadata.csv");
            var movieReader = new CsvReader((IParser)mReader);
            var movies = movieReader.GetRecords<Movie>();

            var stats = from m in movies
                        join mv in movieViews
                            on m.MovieId equals mv.MovieId
                        select new
                        {
                            m.MovieId,
                            m.Title,
                            mv.WatchDurationMs,
                            m.ReleaseYear
                        };


            var fullstats =  from s in stats
                            group s by new { s.MovieId, s.Title, s.ReleaseYear } into g
                            select new 
                            { 
                                g.Key.MovieId, 
                                g.Key.Title, 
                                g.Key.ReleaseYear, 
                                TotalWatch = g.Sum(t => t.WatchDurationMs), 
                                Count = g.Count()  
                            };

            //loop around the list to create list of return objects - MovieStat List
            //TotalWatch divided by Count -> average
            //order list by TotalWatch Desc
            //
            return new List<MovieStat>();
        }

        [HttpPost]
        public IActionResult CreateMovie([FromBody] NewMovie newMovie)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            string newMoviesPath = "../Data/";
            
            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(newMoviesPath, "NewMovies.csv"), true))
            {
                outputFile.WriteLine(JsonConvert.SerializeObject(newMovie));
            }
            return Ok();
        }
    }
}


