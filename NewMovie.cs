﻿using System;

namespace Movie_API.Models
{
    public class NewMovie
    {
        public int MovieId { get; set; }

        public string Title { get; set; }

        public string Language { get; set; }

        public string Duration { get; set; }

        public int ReleaseYear { get; set; }
    }
}