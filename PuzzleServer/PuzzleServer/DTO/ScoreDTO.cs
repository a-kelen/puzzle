using System;

namespace PuzzleServer.DTO
{
    public class ScoreDTO
    {
        public Guid Id { get; set; }
        public int Slides { get; set; }
        public int Seconds { get; set; }
        public DateTime Created { get; set; }
    }
}
