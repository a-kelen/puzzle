using System;
using System.Collections.Generic;

namespace PuzzleServer.Models
{
    public class Backup
    {
        public Guid Id { get; set; }
        public int Slides { get; set; }
        public int Seconds { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Tile> Tiles { get; set; }
    }
}
