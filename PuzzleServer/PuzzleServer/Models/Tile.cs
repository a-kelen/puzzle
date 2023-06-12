using System;

namespace PuzzleServer.Models
{
    public class Tile
    {
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Number { get; set; }
        public Guid BackupId { get; set; }
        public Backup Backup { get; set; }
    }
}
