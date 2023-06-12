using System;
using System.Collections.Generic;

namespace PuzzleServer.DTO
{
    public class BackupDTO
    {
        public Guid Id { get; set; }
        public int Slides { get; set; }
        public int Seconds { get; set; }
        public List<TileDTO> Tiles { get; set; }
    }
}
