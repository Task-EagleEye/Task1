using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_API.Models
{
    public class MovieStat
    {
        public int MovieId { get; set; }

        public string Title { get; set; }

        public int AverageWatchDuration { get; set; }

        public int Watches { get; set; }

        public int ReleaseYear { get; set; }
    }
}
